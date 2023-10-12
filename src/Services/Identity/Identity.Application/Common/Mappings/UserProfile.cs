using AutoMapper;
using Identity.Application.Responses;
using Identity.Shared.Models;

namespace Identity.Application.Common.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserResponse, ApplicationUser>().ReverseMap();

    }
}
