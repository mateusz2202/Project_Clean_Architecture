using Identity.Shared.Models;
using Identity.Shared.Permissions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.Infrastructure.Extensions;

public static class ClaimsExtenstions
{  

    public static async Task<IdentityResult> AddPermissionClaim(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string permission)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
        {
            return await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
        }

        return IdentityResult.Failed();
    }
}