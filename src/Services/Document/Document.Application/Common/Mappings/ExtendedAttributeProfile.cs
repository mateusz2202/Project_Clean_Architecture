using AutoMapper;
using Document.Application.Features.ExtendedAttributes.Commands.AddEdit;
using Document.Application.Features.ExtendedAttributes.Queries.GetAll;
using Document.Application.Features.ExtendedAttributes.Queries.GetAllByEntityId;
using Document.Application.Features.ExtendedAttributes.Queries.GetById;
using Document.Domain.ExtendedAttributes;

namespace Document.Application.Common.Mappings;

public class ExtendedAttributeProfile : Profile
{
    public ExtendedAttributeProfile()
    {
        CreateMap(typeof(AddEditExtendedAttributeCommand<,,,>), typeof(DocumentExtendedAttribute))
            .ForMember(nameof(DocumentExtendedAttribute.Entity), opt => opt.Ignore())
            .ForMember(nameof(DocumentExtendedAttribute.CreatedBy), opt => opt.Ignore())
            .ForMember(nameof(DocumentExtendedAttribute.CreatedOn), opt => opt.Ignore())
            .ForMember(nameof(DocumentExtendedAttribute.ModifiedBy), opt => opt.Ignore())
            .ForMember(nameof(DocumentExtendedAttribute.ModifiedOn), opt => opt.Ignore());

        CreateMap(typeof(GetExtendedAttributeByIdResponse<,>), typeof(DocumentExtendedAttribute)).ReverseMap();
        CreateMap(typeof(GetAllExtendedAttributesResponse<,>), typeof(DocumentExtendedAttribute)).ReverseMap();
        CreateMap(typeof(GetAllExtendedAttributesByEntityIdResponse<,>), typeof(DocumentExtendedAttribute)).ReverseMap();
    }
}
