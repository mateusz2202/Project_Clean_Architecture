using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Identity.RoleClaims;

public class RoleClaimManager : IRoleClaimManager
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RoleClaimManager(IHttpClientFactory httpClientFactory)
    {            
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IResult<string>> DeleteAsync(string id)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
        var response = await httpClient.DeleteAsync($"{Routes.RoleClaimsEndpoints.Delete}/{id}");
        return await response.ToResult<string>();
    }

    public async Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync()
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
        var response = await httpClient.GetAsync(Routes.RoleClaimsEndpoints.GetAll);
        return await response.ToResult<List<RoleClaimResponse>>();
    }

    public async Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
        var response = await httpClient.GetAsync($"{Routes.RoleClaimsEndpoints.GetAll}/{roleId}");
        return await response.ToResult<List<RoleClaimResponse>>();
    }

    public async Task<IResult<string>> SaveAsync(RoleClaimRequest role)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);
        var response = await httpClient.PostAsJsonAsync(Routes.RoleClaimsEndpoints.Save, role);
        return await response.ToResult<string>();
    }
}