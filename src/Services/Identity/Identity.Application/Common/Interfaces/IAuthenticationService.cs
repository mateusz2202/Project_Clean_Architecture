using Identity.Application.Models.Authentication;
using Identity.Application.Responses;
using Identity.Shared.Wrapper;

namespace Identity.Application.Common.Interfaces;

public interface IAuthenticationService
{
    Task<Result<TokenResponse>> AuthenticateAsync(AuthenticationRequest request);
    Task<IResult> RegisterAsync(RegistrationRequest request);
}
