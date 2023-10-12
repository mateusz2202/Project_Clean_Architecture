using Identity.Application.Models.Authentication;
using Identity.Shared.Wrapper;

namespace Identity.Application.Common.Interfaces;

public interface IAuthenticationService
{
    Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
    Task<IResult> RegisterAsync(RegistrationRequest request);
}
