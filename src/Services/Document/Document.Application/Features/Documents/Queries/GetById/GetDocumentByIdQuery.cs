using AutoMapper;
using Document.Application.Interfaces.Repositories;
using Document.Shared.Wrapper;
using MediatR;

namespace Document.Application.Features.Documents.Queries.GetById;

public record GetDocumentByIdQuery(int Id) : IRequest<Result<GetDocumentByIdResponse>>;


internal class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, Result<GetDocumentByIdResponse>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IMapper _mapper;

    public GetDocumentByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetDocumentByIdResponse>> Handle(GetDocumentByIdQuery query, CancellationToken cancellationToken)
    {
        var document = await _unitOfWork.Repository<Domain.Entities.Document>().GetByIdAsync(query.Id);
        var mappedDocument = _mapper.Map<GetDocumentByIdResponse>(document);
        return await Result<GetDocumentByIdResponse>.SuccessAsync(mappedDocument);
    }
}
