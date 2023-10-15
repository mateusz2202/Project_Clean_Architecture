using AutoMapper;
using Product.Application.Application.Features.Products.Commands.AddEdit;

namespace Product.Application.Common.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<AddEditProductCommand, Domain.Entities.Product>().ReverseMap();
    }
}
