using API_Identity.Interfaces;
using API_Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Identity.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;
    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var token = await _userService.GetToken(dto);
        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("registeruser")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO dto)
    {
        await _userService.RegisterUser(dto);
        return Ok();
    }


    [HttpPut("changepassword")]
    public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordDTO dto)
    {
        await _userService.ChangePassword(dto);
        return Ok();
    }

}
