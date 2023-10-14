using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Routes;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Identity.Users;

public class UserManager : IUserManager
{
    private readonly IHttpClientFactory _httpClientFactory;
    public UserManager(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient IdentityClient() => _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);

    public async Task<IResult<List<UserResponse>>> GetAllAsync()
        => await IdentityClient().GetResult<List<UserResponse>>(UserEndpoints.GetAll);

    public async Task<IResult<UserResponse>> GetAsync(string userId)
        => await IdentityClient().GetResult<UserResponse>(UserEndpoints.Get(userId));

    public async Task<IResult> RegisterUserAsync(RegisterRequest request)
        => await IdentityClient().PostAsJsonResult<IResult, RegisterRequest>(AccountEndpoints.Register, request);

    public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        => await IdentityClient().PostAsJsonResult<IResult, ToggleUserStatusRequest>(UserEndpoints.ToggleUserStatus, request);

    public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
        => await IdentityClient().GetResult<UserRolesResponse>(UserEndpoints.GetUserRoles(userId));

    public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
        => await IdentityClient().PuttAsJsonResult<UserRolesResponse, UpdateUserRolesRequest>(UserEndpoints.GetUserRoles(request.UserId), request);

    public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest model)
        => await IdentityClient().PostAsJsonResult<IResult, ForgotPasswordRequest>(UserEndpoints.ForgotPassword, model);

    public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        => await IdentityClient().PostAsJsonResult<IResult, ResetPasswordRequest>(UserEndpoints.ResetPassword, request);

    public async Task<string> ExportToExcelAsync(string searchString = "")
        => await (await IdentityClient().GetAsync(UserEndpoints.Export(searchString))).Content.ReadAsStringAsync();

}