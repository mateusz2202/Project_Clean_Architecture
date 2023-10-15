using AutoMapper;
using BlazorApp.Application.Requests.Identity;
using BlazorApp.Application.Responses.Identity;

namespace BlazorApp.Client.Infrastructure.Mappings;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<PermissionResponse, PermissionRequest>().ReverseMap();
        CreateMap<RoleClaimResponse, RoleClaimRequest>().ReverseMap();       
    }
}