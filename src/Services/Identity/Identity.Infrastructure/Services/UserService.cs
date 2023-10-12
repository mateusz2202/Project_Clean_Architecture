using AutoMapper;
using Identity.Application.Common.Exceptions;
using Identity.Application.Common.Interfaces;
using Identity.Application.Requests;
using Identity.Application.Responses;
using Identity.Infrastructure.Models;
using Identity.Shared;
using Identity.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Encodings.Web;

namespace Identity.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        RoleManager<ApplicationRole> roleManager,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<UserResponse>>> GetAllAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var result = _mapper.Map<List<UserResponse>>(users);
        return await Result<List<UserResponse>>.SuccessAsync(result);
    }

    public async Task<IResult> RegisterAsync(RegisterRequest request, string origin)
    {
        var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
        if (userWithSameUserName != null)
        {
            return await Result.FailAsync(string.Format("Username {0} is already taken.", request.UserName));
        }
        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            PhoneNumber = request.PhoneNumber,
            EmailConfirmed = request.AutoConfirmEmail
        };

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (userWithSamePhoneNumber != null)
            {
                return await Result.FailAsync(string.Format("Phone number {0} is already registered.", request.PhoneNumber));
            }
        }

        return await Result<string>.SuccessAsync(user.Id, string.Format("User {0} Registered.", user.UserName));
    }

    public async Task<IResult<UserResponse>> GetAsync(string userId)
    {
        var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        var result = _mapper.Map<UserResponse>(user);
        return await Result<UserResponse>.SuccessAsync(result);
    }

    public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
    {
        var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
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
        var user = await _userManager.FindByIdAsync(userId);
        var roles = await _roleManager.Roles.ToListAsync();

        foreach (var role in roles)
        {
            var userRolesViewModel = new UserRoleModel
            {
                RoleName = role.Name,              
            };
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                userRolesViewModel.Selected = true;
            }
            else
            {
                userRolesViewModel.Selected = false;
            }
            viewModel.Add(userRolesViewModel);
        }
        var result = new UserRolesResponse { UserRoles = viewModel };
        return await Result<UserRolesResponse>.SuccessAsync(result);
    }

    public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user.Email == "mukesh@blazorhero.com")
        {
            return await Result.FailAsync("Not Allowed.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var selectedRoles = request.UserRoles.Where(x => x.Selected).ToList();

        var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
        if (!await _userManager.IsInRoleAsync(currentUser,ApplicationConstans.RoleConstants.AdministratorRole))
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
        var user = await _userManager.FindByIdAsync(userId);
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            return await Result<string>.SuccessAsync(user.Id, string.Format("Account Confirmed for {0}. You can now use the /api/identity/token endpoint to generate JWT.", user.Email));
        }
        else
        {
            throw new BadRequestException(string.Format("An error occurred while confirming {0}", user.Email));
        }
    }

    public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            // Don't reveal that the user does not exist or is not confirmed
            return await Result.FailAsync("An Error has occurred!");
        }
        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var route = "account/reset-password";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var passwordResetURL = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
        var mailRequest = new MailRequest
        {
            Body = string.Format("Please reset your password by <a href='{0}'>clicking here</a>.", HtmlEncoder.Default.Encode(passwordResetURL)),
            Subject = "Reset Password",
            To = request.Email
        };
        return await Result.SuccessAsync("Password Reset Mail has been sent to your authorized Email.");
    }

    public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            return await Result.FailAsync("An Error has occured!");
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (result.Succeeded)
        {
            return await Result.SuccessAsync("Password Reset Successful!");
        }
        else
        {
            return await Result.FailAsync("An Error has occured!");
        }
    }

    public async Task<int> GetCountAsync()
    {
        var count = await _userManager.Users.CountAsync();
        return count;
    }
   
}
