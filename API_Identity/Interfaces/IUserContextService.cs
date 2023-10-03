using System.Security.Claims;

namespace API_Identity.Interfaces;

public interface IUserContextService
{
    string? Email { get; }
    ClaimsPrincipal? User { get; }
}
