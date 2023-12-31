﻿using BlazorApp.Application.Requests.Identity;
using BlazorApp.Application.Responses.Identity;
using BlazorApp.Client.Infrastructure.Extensions;
using BlazorApp.Shared.Constants.Application;
using BlazorApp.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp.Client.Infrastructure.Managers.Identity.Roles;

public class RoleManager : IRoleManager
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RoleManager(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient IdentityClient() => _httpClientFactory.CreateClient(ApplicationConstants.ClientApi.IdentityClient);

    public async Task<IResult<string>> DeleteAsync(string id)
        => await IdentityClient().DeleteResult<string>($"{Routes.RolesEndpoints.Delete}/{id}");

    public async Task<IResult<List<RoleResponse>>> GetRolesAsync()
        => await IdentityClient().GetResult<List<RoleResponse>>(Routes.RolesEndpoints.GetAll);

    public async Task<IResult<string>> SaveAsync(RoleRequest role)
        => await IdentityClient().PostAsJsonResult<string, RoleRequest>(Routes.RolesEndpoints.Save, role);

    public async Task<IResult<PermissionResponse>> GetPermissionsAsync(string roleId)
       => await IdentityClient().GetResult<PermissionResponse>(Routes.RolesEndpoints.GetPermissions + roleId);

    public async Task<IResult<string>> UpdatePermissionsAsync(PermissionRequest request)
        => await IdentityClient().PuttAsJsonResult<string, PermissionRequest>(Routes.RolesEndpoints.UpdatePermissions, request);


}