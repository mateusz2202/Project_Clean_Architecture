using Identity.Application.Common.Interfaces;
using Identity.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("api/identity/roleClaim")]
[ApiController]
public class RoleClaimController : ControllerBase
{
    private readonly IRoleClaimService _roleClaimService;

    public RoleClaimController(IRoleClaimService roleClaimService)
    {
        _roleClaimService = roleClaimService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _roleClaimService.GetAllAsync());


    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetAllByRoleId([FromRoute] string roleId)
        => Ok(await _roleClaimService.GetAllByRoleIdAsync(roleId));



    [HttpPost]
    public async Task<IActionResult> Post(RoleClaimRequest request)
        => Ok(await _roleClaimService.SaveAsync(request));



    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _roleClaimService.DeleteAsync(id));

}
