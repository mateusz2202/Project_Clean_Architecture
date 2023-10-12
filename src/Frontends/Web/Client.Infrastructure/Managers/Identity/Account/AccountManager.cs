using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Identity.Account;

public class AccountManager : IAccountManager
{       
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountManager(IHttpClientFactory httpClientFactory)
    {           
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
        var response = await httpClient.PutAsJsonAsync(Routes.UserEndpoints.ChangePassword, model);
        return await response.ToResult();
    }

    public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest model)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
        var response = await httpClient.PutAsJsonAsync(Routes.UserEndpoints.UpdateProfile, model);
        return await response.ToResult();
    }

    public async Task<IResult<string>> GetProfilePictureAsync(string userId)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
        var response = await httpClient.GetAsync(Routes.UserEndpoints.GetProfilePicture(userId));
        return await response.ToResult<string>();
    }

    public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
        var response = await httpClient.PostAsJsonAsync(Routes.UserEndpoints.UpdateProfilePicture(userId), request);
        return await response.ToResult<string>();
    }
}