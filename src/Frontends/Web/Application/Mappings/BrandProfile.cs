using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.Brands;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Application.Mappings;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<AddEditBrandCommand, Brand>().ReverseMap();       
        CreateMap<GetAllBrandsResponse, Brand>().ReverseMap();
    }
}