using Identity.Application.Common.Interfaces;
using Identity.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("api/identity/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _userService.GetAllAsync());


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
        => Ok(await _userService.GetAsync(id));


    [HttpGet("roles/{id}")]
    public async Task<IActionResult> GetRolesAsync(string id)
        => Ok(await _userService.GetRolesAsync(id));


    [HttpPut("roles/{id}")]
    public async Task<IActionResult> UpdateRolesAsync(UpdateUserRolesRequest request)
        => Ok(await _userService.UpdateRolesAsync(request));


    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        => Ok(await _userService.ConfirmEmailAsync(userId, code));


    [HttpPost("toggle-status")]
    public async Task<IActionResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        => Ok(await _userService.ToggleUserStatusAsync(request));  

  
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        => Ok(await _userService.ResetPasswordAsync(request));    

}
