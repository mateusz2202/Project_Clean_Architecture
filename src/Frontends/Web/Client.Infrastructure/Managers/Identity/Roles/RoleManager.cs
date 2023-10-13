using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Identity.Roles
{
    public class RoleManager : IRoleManager
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RoleManager(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IResult<string>> DeleteAsync(string id)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.DeleteAsync($"{Routes.RolesEndpoints.Delete}/{id}");
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<RoleResponse>>> GetRolesAsync()
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.GetAsync(Routes.RolesEndpoints.GetAll);
            return await response.ToResult<List<RoleResponse>>();
        }

        public async Task<IResult<string>> SaveAsync(RoleRequest role)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.PostAsJsonAsync(Routes.RolesEndpoints.Save, role);
            return await response.ToResult<string>();
        }

        public async Task<IResult<PermissionResponse>> GetPermissionsAsync(string roleId)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.GetAsync(Routes.RolesEndpoints.GetPermissions + roleId);
            return await response.ToResult<PermissionResponse>();
        }

        public async Task<IResult<string>> UpdatePermissionsAsync(PermissionRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
            var response = await httpClient.PutAsJsonAsync(Routes.RolesEndpoints.UpdatePermissions, request);
            return await response.ToResult<string>();
        }
    }
}