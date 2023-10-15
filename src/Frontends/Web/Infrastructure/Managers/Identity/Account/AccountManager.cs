using BlazorApp.Application.Requests.Identity;
using BlazorApp.Client.Infrastructure.Extensions;
using BlazorApp.Shared.Constants.Application;
using BlazorApp.Shared.Wrapper;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp.Client.Infrastructure.Managers.Identity.Account;

public class AccountManager : IAccountManager
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountManager(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    private HttpClient IdentityClient() => _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);

    public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model)
       => await IdentityClient().PostAsJsonResult<IResult, ChangePasswordRequest>(Routes.UserEndpoints.ChangePassword, model);

    public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest model)
        => await IdentityClient().PuttAsJsonResult<IResult, UpdateProfileRequest>(Routes.UserEndpoints.UpdateProfile, model);

    public async Task<IResult<string>> GetProfilePictureAsync(string userId)
        => await IdentityClient().GetResult<string>(Routes.UserEndpoints.GetProfilePicture(userId));


    public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
        => await IdentityClient().PostAsJsonResult<string, UpdateProfilePictureRequest>(Routes.UserEndpoints.UpdateProfilePicture(userId), request);


}