using Identity.Application.Common.Exceptions;
using Identity.Application.Common.Interfaces;
using Identity.Application.Models.Authentication;
using Identity.Infrastructure.Models;
using Identity.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtSettings _jwtSettings;

    public AuthenticationService(UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _signInManager = signInManager;
    }

    public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
    {
        ApplicationUser user = await _userManager.FindByEmailAsync(request.Email)
                               ?? throw new NotFoundException($"User with {request.Email} not found.");

        if (!user.EmailConfirmed)
            throw new BadRequestException("E-Mail not confirmed");

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordValid)
            throw new BadRequestException("Invalid Credentials");


        var result = await _signInManager.PasswordSignInAsync(user.UserName ?? string.Empty, request.Password, false, lockoutOnFailure: false);

        if (!result.Succeeded)
            throw new Exception($"Credentials for '{request.Email} aren't valid'.");


        JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

        AuthenticationResponse response = new()
        {
            Id = user.Id,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty
        };

        return response;
    }

    public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
    {
        var existingUser = await _userManager.FindByNameAsync(request.UserName);

        if (existingUser != null)
            throw new Exception($"Username '{request.UserName}' already exists.");


        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            EmailConfirmed = false
        };

        var existingEmail = await _userManager.FindByEmailAsync(request.Email);

        if (existingEmail == null)
        {
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, ApplicationConstans.RoleConstants.BasicRole);
                return new RegistrationResponse() { UserId = user.Id };
            }
            else
                throw new ValidationException(result.Errors);

        }
        else
        {
            throw new BadRequestException($"Email {request.Email} already exists.");
        }
    }

    private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = roles.Select(x => new Claim("roles", x));

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user?.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user?.Email ?? string.Empty),
                new Claim("uid", user?.Id ?? string.Empty)
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }
}
