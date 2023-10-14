using BlazorHero.CleanArchitecture.Application.Interfaces.Common;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;

public interface IUserService : IService
{

    Task<IResult<UserResponse>> GetAsync(string userId);

    Task<IResult<UserRolesResponse>> GetRolesAsync(string id);
   
}