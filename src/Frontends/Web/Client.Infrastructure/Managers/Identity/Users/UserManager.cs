using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Routes;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Identity.Users
{
    public class UserManager : IUserManager
    {      
        private readonly IHttpClientFactory _httpClientFactory;
        public UserManager(IHttpClientFactory httpClientFactory)
        {            
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IResult<List<UserResponse>>> GetAllAsync()
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.GetAsync(UserEndpoints.GetAll);
            return await response.ToResult<List<UserResponse>>();
        }

        public async Task<IResult<UserResponse>> GetAsync(string userId)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.GetAsync(UserEndpoints.Get(userId));          
            return await response.ToResult<UserResponse>();
        }

        public async Task<IResult> RegisterUserAsync(RegisterRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);                   
            var response = await httpClient.PostAsJsonAsync(AccountEndpoints.Register, request);
            return await response.ToResult();
        }

        public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.PostAsJsonAsync(UserEndpoints.ToggleUserStatus, request);
            return await response.ToResult();
        }

        public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.GetAsync(UserEndpoints.GetUserRoles(userId));
            return await response.ToResult<UserRolesResponse>();
        }

        public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.PutAsJsonAsync(UserEndpoints.GetUserRoles(request.UserId), request);
            return await response.ToResult<UserRolesResponse>();
        }

        public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest model)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.PostAsJsonAsync(UserEndpoints.ForgotPassword, model);
            return await response.ToResult();
        }

        public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.PostAsJsonAsync(UserEndpoints.ResetPassword, request);
            return await response.ToResult();
        }

        public async Task<string> ExportToExcelAsync(string searchString = "")
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? UserEndpoints.Export
                : UserEndpoints.ExportFiltered(searchString));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
    }
}