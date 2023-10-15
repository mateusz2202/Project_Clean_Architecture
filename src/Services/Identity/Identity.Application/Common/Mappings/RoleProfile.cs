using AutoMapper;
using Identity.Application.Requests;
using Identity.Application.Responses;
using Identity.Shared.Models;

namespace Identity.Application.Common.Mappings;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleResponse, ApplicationRole>().ReverseMap();

        CreateMap<RoleClaimResponse, ApplicationRoleClaim>()
           .ForMember(nameof(ApplicationRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
           .ForMember(nameof(ApplicationRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
           .ReverseMap();

        CreateMap<RoleClaimRequest, ApplicationRoleClaim>()
            .ForMember(nameof(ApplicationRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
            .ForMember(nameof(ApplicationRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
            .ReverseMap();
    }
}
