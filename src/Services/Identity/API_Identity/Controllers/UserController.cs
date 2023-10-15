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


    [HttpGet("profile-picture/{userId}")]
    [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
    public async Task<IActionResult> GetProfilePictureAsync(string userId)
        => Ok(await _userService.GetProfilePictureAsync(userId));


    [HttpPost("profile-picture/{userId}")]
    public async Task<IActionResult> UpdateProfilePictureAsync(UpdateProfilePictureRequest request)
        => Ok(await _userService.UpdateProfilePictureAsync(request));


    [HttpPut(nameof(ChangePassword))]
    public async Task<ActionResult> ChangePassword(ChangePasswordRequest request)
        => Ok(await _userService.ChangePasswordAsync(request));


    [HttpPut(nameof(UpdateProfile))]
    public async Task<ActionResult> UpdateProfile(UpdateProfileRequest request)
        => Ok(await _userService.UpdateProfileAsync(request));

}
