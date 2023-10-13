using Identity.Application.Requests;
using Identity.Application.Responses;
using Identity.Shared.Wrapper;

namespace Identity.Application.Common.Interfaces;

public interface IRoleClaimService 
{
    Task<Result<List<RoleClaimResponse>>> GetAllAsync();

    Task<int> GetCountAsync();

    Task<Result<RoleClaimResponse>> GetByIdAsync(int id);

    Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);

    Task<Result<string>> SaveAsync(RoleClaimRequest request);

    Task<Result<string>> DeleteAsync(int id);
}
