using AutoMapper;
using Document.Application.Features.DocumentTypes.Commands.AddEdit;
using Document.Application.Features.DocumentTypes.Queries.GetAll;
using Document.Application.Features.DocumentTypes.Queries.GetById;
using Document.Domain.Entities;

namespace Document.Application.Common.Mappings;

public class DocumentTypeProfile : Profile
{
    public DocumentTypeProfile()
    {
        CreateMap<AddEditDocumentTypeCommand,DocumentType>().ReverseMap();
        CreateMap<GetDocumentTypeByIdResponse, DocumentType>().ReverseMap();
        CreateMap<GetAllDocumentTypesResponse, DocumentType>().ReverseMap();
    }
}
