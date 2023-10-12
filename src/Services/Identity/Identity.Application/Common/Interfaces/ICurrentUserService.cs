using System.Security.Claims;


namespace Identity.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? Email { get; }
    ClaimsPrincipal? User { get; }
    public string UserId { get; }
}

