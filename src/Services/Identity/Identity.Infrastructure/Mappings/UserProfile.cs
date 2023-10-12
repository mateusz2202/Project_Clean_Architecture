using AutoMapper;
using Identity.Application.Responses;
using Identity.Infrastructure.Models;

namespace Identity.Infrastructure.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserResponse, ApplicationUser>().ReverseMap();
      
    }
}
