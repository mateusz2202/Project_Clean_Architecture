using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Azure.Cosmos;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Operation.APIOperation.API_Test;

public class FakeUserFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub,"ds" ),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, "xd@xd.pl"),
                new Claim("uid", "1")
        };
    
        claimsPrincipal.AddIdentity(new ClaimsIdentity(claims));

        context.HttpContext.User = claimsPrincipal;

        await next();
    }
}
