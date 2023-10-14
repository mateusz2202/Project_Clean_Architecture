using AutoMapper;
using Document.Application.Features.Documents.Commands.AddEdit;
using Document.Application.Features.Documents.Queries.GetById;

namespace Document.Application.Common.Mappings;

public class DocumentProfile : Profile
{
    public DocumentProfile()
    {
        CreateMap<AddEditDocumentCommand,Domain.Entities.Document>().ReverseMap();
        CreateMap<GetDocumentByIdResponse, Domain.Entities.Document>().ReverseMap();
    }
}
