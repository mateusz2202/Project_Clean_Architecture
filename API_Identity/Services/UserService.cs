using API_Identity.Data;
using API_Identity.Exceptions;
using API_Identity.Interfaces;
using API_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Identity.Services;

public class UserService : IUserService
{
    //private readonly IndentityDbContext _indentityDbContext;
    //private readonly AuthenticationSettings _authenticationSettings;
    //private readonly IPasswordHasher<User> _passwordHasher;
    //public UserService(
    //    IndentityDbContext indentityDbContext,
    //    AuthenticationSettings authenticationSettings,
    //    IPasswordHasher<User> passwordHasher)
    //{
    //    _indentityDbContext = indentityDbContext;
    //    _authenticationSettings = authenticationSettings;
    //    _passwordHasher = passwordHasher;
    //}

    //public async Task<string> GetToken(LoginDTO? dto)
    //{
    //    var user = await _indentityDbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == dto?.Email?.ToLower())
    //                 ?? throw new BadRequestException("Incorect email or password");   
    //    var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto?.Password);
    //    if (result == PasswordVerificationResult.Failed) throw new BadRequestException("Incorect email or password");
    //    var claims = new List<Claim>()
    //        {
    //            new Claim(ClaimTypes.NameIdentifier, user.Name),
    //            new Claim(ClaimTypes.Email,user.Email),
    //            new Claim("EmailConfirmed", "true")
    //        };
    //    if (_authenticationSettings.JwtKey == null) throw new Exception();
    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
    //    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    //    var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
    //    var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
    //        _authenticationSettings.JwtIssuer,
    //        claims,
    //        expires: expires,
    //        signingCredentials: cred);
    //    return new JwtSecurityTokenHandler().WriteToken(token);
    //}  


    //public async Task RegisterUser(RegisterUserDTO? dto)
    //{
    //    var user = new User()
    //    {
    //        Name = dto.Name,
    //        Email = dto.Email,
    //        IsActive = false,        
    //        CreateDate = DateTime.Now,
    //    };
    //    var hashedPassword = _passwordHasher.HashPassword(user, dto?.Password);
    //    user.Password = hashedPassword;
    //    _indentityDbContext.Users?.Add(user);
    //    await _indentityDbContext.SaveChangesAsync();    
    //}

    //public async Task ChangePassword(string email, UpdatePasswordDTO? dto)
    //{
    //    var user = await _indentityDbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == dto?.Email?.ToLower())
    //                ?? throw new BadRequestException("Incorect email or password");
    //    var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto?.OldPassword);
    //    if (result == PasswordVerificationResult.Failed) throw new BadRequestException("Incorect email or password");
    //    var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
    //    user.Password = hashedPassword;
    //    _indentityDbContext.Users.Update(user);
    //    await _indentityDbContext.SaveChangesAsync();
    //}
    public async Task ChangePassword(string email, UpdatePasswordDTO dto)
    {
        await Task.CompletedTask;
    }

    public async Task<string> GetToken(LoginDTO dto)
    {
        return await Task.FromResult<string>("xd");
    }

    public async Task RegisterUser(RegisterUserDTO dto)
    {
        await Task.CompletedTask;
    }
}
