using API_Identity.Interfaces;
using System.Security.Claims;

namespace API_Identity.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor contextAccessor)
    {
        _httpContextAccessor = contextAccessor;
    }

    public ClaimsPrincipal? User
        => _httpContextAccessor.HttpContext?.User;

    public string? Email
        => User?.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;

}

