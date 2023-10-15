using AutoMapper;
using Identity.Application.Common.Exceptions;
using Identity.Application.Common.Interfaces;
using Identity.Application.Requests;
using Identity.Application.Responses;
using Identity.Shared.Models;
using Identity.Shared;
using Identity.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Identity.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUploadService _uploadService;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ICurrentUserService currentUserService,
        IUploadService uploadService,
        IMapper mapper,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
        _currentUserService = currentUserService;
        _uploadService = uploadService;
        _signInManager = signInManager;
    }

    public async Task<Result<List<UserResponse>>> GetAllAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var result = _mapper.Map<List<UserResponse>>(users);
        return await Result<List<UserResponse>>.SuccessAsync(result);
    }  

    public async Task<IResult<UserResponse>> GetAsync(string userId)
    {
        var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        var result = _mapper.Map<UserResponse>(user);
        return await Result<UserResponse>.SuccessAsync(result);
    }

    public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
    {
        var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync() ?? throw new NotFoundException("Not found user"); ;
        var isAdmin = await _userManager.IsInRoleAsync(user, ApplicationConstans.RoleConstants.AdministratorRole);
        if (isAdmin)
        {
            return await Result.FailAsync("Administrators Profile's Status cannot be toggled");
        }
        if (user != null)
        {
            var identityResult = await _userManager.UpdateAsync(user);
        }
        return await Result.SuccessAsync();
    }

    public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
    {
        var viewModel = new List<UserRoleModel>();
        var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException("Not found user");
        
        var roles = await _roleManager.Roles.ToListAsync(); 

        foreach (var role in roles)
        {
            var userRolesViewModel = new UserRoleModel
            {
                RoleName = role?.Name ?? string.Empty,
                RoleDescription = role?.Description ?? string.Empty,
            };
            
            if (await _userManager.IsInRoleAsync(user, role?.Name ?? string.Empty))            
                userRolesViewModel.Selected = true;            
            else            
                userRolesViewModel.Selected = false;
            
            viewModel.Add(userRolesViewModel);
        }
        var result = new UserRolesResponse { UserRoles = viewModel };

        return await Result<UserRolesResponse>.SuccessAsync(result);
    }

    public async Task<IResult<string>> GetProfilePictureAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return await Result<string>.FailAsync("User Not Found");
        }
        return await Result<string>.SuccessAsync(data: user.ProfilePictureDataUrl);
    }

    public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId) ?? throw new NotFoundException("Not found user"); ;
        
        if (user.Email == "jmate220@gmail.com")
        {
            return await Result.FailAsync("Not Allowed.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var selectedRoles = request.UserRoles.Where(x => x.Selected).ToList();

        var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId) ?? throw new NotFoundException("Not found user");

        if (!await _userManager.IsInRoleAsync(currentUser, ApplicationConstans.RoleConstants.AdministratorRole))
        {
            var tryToAddAdministratorRole = selectedRoles
                .Any(x => x.RoleName == ApplicationConstans.RoleConstants.AdministratorRole);
            var userHasAdministratorRole = roles.Any(x => x == ApplicationConstans.RoleConstants.AdministratorRole);
            if (tryToAddAdministratorRole && !userHasAdministratorRole || !tryToAddAdministratorRole && userHasAdministratorRole)
            {
                return await Result.FailAsync("Not Allowed to add or delete Administrator Role if you have not this role.");
            }
        }

        var result = await _userManager.RemoveFromRolesAsync(user, roles);
        result = await _userManager.AddToRolesAsync(user, selectedRoles.Select(y => y.RoleName));

        return await Result.SuccessAsync("Roles Updated");
    }

    public async Task<IResult<string>> ConfirmEmailAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException("Not found user");
        
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        
        var result = await _userManager.ConfirmEmailAsync(user, code);
        
        if (result.Succeeded)        
            return await Result<string>
                .SuccessAsync(
                user.Id, 
                string.Format("Account Confirmed for {0}. You can now use the /api/identity/token endpoint to generate JWT.",
                user.Email));        
        else        
            throw new BadRequestException(string.Format("An error occurred while confirming {0}", user.Email));
        
    }

    public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new NotFoundException("Not found user");

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

        if (result.Succeeded)        
            return await Result.SuccessAsync("Password Reset Successful!");        
        else        
            return await Result.FailAsync("An Error has occured!");
        
    }

    public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request)
    {
        var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
        if (user == null) 
            return await Result<string>.FailAsync(message: "User Not Found");
        var filePath = _uploadService.UploadAsync(request);
        user.ProfilePictureDataUrl = filePath;
        var identityResult = await _userManager.UpdateAsync(user);
        var errors = identityResult.Errors.Select(e => e.Description.ToString()).ToList();
        return identityResult.Succeeded ? await Result<string>.SuccessAsync(data: filePath) : await Result<string>.FailAsync(errors);
    }

    public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model)
    {
        var user = await this._userManager.FindByIdAsync(_currentUserService.UserId);
        if (user == null)        
            return await Result.FailAsync("User Not Found.");        

        var identityResult = await this._userManager.ChangePasswordAsync(
            user,
            model.Password,
            model.NewPassword);
        var errors = identityResult.Errors.Select(e => e.Description.ToString()).ToList();
        return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
    }

    public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (userWithSamePhoneNumber != null)
            {
                return await Result.FailAsync(string.Format("Phone number {0} is already used.", request.PhoneNumber));
            }
        }

        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail == null || userWithSameEmail.Id == _currentUserService.UserId)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (user == null)
            {
                return await Result.FailAsync("User Not Found.");
            }
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (request.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
            }
            var identityResult = await _userManager.UpdateAsync(user);
            var errors = identityResult.Errors.Select(e => e.Description.ToString()).ToList();
            await _signInManager.RefreshSignInAsync(user);
            return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
        }
        else
        {
            return await Result.FailAsync(string.Format("Email {0} is already used.", request.Email));
        }
    }



}
