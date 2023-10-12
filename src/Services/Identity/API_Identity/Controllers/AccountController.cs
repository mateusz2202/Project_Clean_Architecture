using Identity.Application.Common.Interfaces;
using Identity.Application.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("api/identity/account")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    public AccountController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<ActionResult> Login(AuthenticationRequest request)
        => Ok(await _authenticationService.AuthenticateAsync(request));


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<RegistrationResponse>> RegisterUser(RegistrationRequest request)
        => Ok(await _authenticationService.RegisterAsync(request));   

}
