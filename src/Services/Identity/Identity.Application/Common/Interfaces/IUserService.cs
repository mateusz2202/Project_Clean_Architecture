using Identity.Application.Requests;
using Identity.Application.Responses;
using Identity.Shared.Wrapper;

namespace Identity.Application.Common.Interfaces;

public interface IUserService 
{
    Task<Result<List<UserResponse>>> GetAllAsync();

    Task<int> GetCountAsync();

    Task<IResult<UserResponse>> GetAsync(string userId);

    Task<IResult> RegisterAsync(RegisterRequest request, string origin);

    Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request);

    Task<IResult<UserRolesResponse>> GetRolesAsync(string id);

    Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request);

    Task<IResult<string>> ConfirmEmailAsync(string userId, string code);

    Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

    Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
}
