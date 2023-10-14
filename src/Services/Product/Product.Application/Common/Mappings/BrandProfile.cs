using AutoMapper;
using Product.Application.Application.Features.Brands.Commands.AddEdit;
using Product.Application.Application.Features.Brands.Queries.GetAll;
using Product.Application.Application.Features.Brands.Queries.GetById;
using Product.Domain.Entities;

namespace Product.Application.Common.Mappings;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<AddEditBrandCommand, Brand>().ReverseMap();
        CreateMap<GetBrandByIdResponse, Brand>().ReverseMap();
        CreateMap<GetAllBrandsResponse, Brand>().ReverseMap();
    }
}
