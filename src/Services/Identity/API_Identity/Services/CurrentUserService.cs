using Identity.Application.Common.Interfaces;
using System.Security.Claims;

namespace API_Identity.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        _httpContextAccessor = contextAccessor;
    }

    public ClaimsPrincipal? User
        => _httpContextAccessor.HttpContext?.User;

    public string? Email
        => User?.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;

}

