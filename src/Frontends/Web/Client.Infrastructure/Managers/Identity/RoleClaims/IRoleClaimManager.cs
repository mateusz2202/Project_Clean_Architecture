using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Application.Requests.Identity;
using BlazorApp.Application.Responses.Identity;
using BlazorApp.Shared.Wrapper;

namespace BlazorApp.Client.Infrastructure.Managers.Identity.RoleClaims
{
    public interface IRoleClaimManager : IManager
    {
        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync();

        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId);

        Task<IResult<string>> SaveAsync(RoleClaimRequest role);

        Task<IResult<string>> DeleteAsync(string id);
    }
}