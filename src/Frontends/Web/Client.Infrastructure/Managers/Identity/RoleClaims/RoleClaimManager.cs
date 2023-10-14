using System.Collections.Generic;
using System.Net.Http;
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
    private HttpClient IdentityClient() => _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);

    public async Task<IResult<string>> DeleteAsync(string id)
        => await IdentityClient().DeleteResult<string>($"{Routes.RoleClaimsEndpoints.Delete}/{id}");

    public async Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync()
        => await IdentityClient().GetResult<List<RoleClaimResponse>>(Routes.RoleClaimsEndpoints.GetAll);

    public async Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId)
        => await IdentityClient().GetResult<List<RoleClaimResponse>>($"{Routes.RoleClaimsEndpoints.GetAll}/{roleId}");

    public async Task<IResult<string>> SaveAsync(RoleClaimRequest role)
        => await IdentityClient().PostAsJsonResult<string, RoleClaimRequest>(Routes.RoleClaimsEndpoints.Save, role);


}