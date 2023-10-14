using AutoMapper;
using Document.Application.Interfaces.Repositories;
using Document.Domain.Entities;
using Document.Shared.Wrapper;
using MediatR;

namespace Document.Application.Features.DocumentTypes.Queries.GetById;

public record GetDocumentTypeByIdQuery(int Id) : IRequest<Result<GetDocumentTypeByIdResponse>>;

internal class GetDocumentTypeByIdQueryHandler : IRequestHandler<GetDocumentTypeByIdQuery, Result<GetDocumentTypeByIdResponse>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IMapper _mapper;

    public GetDocumentTypeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetDocumentTypeByIdResponse>> Handle(GetDocumentTypeByIdQuery query, CancellationToken cancellationToken)
    {
        var documentType = await _unitOfWork.Repository<DocumentType>().GetByIdAsync(query.Id);
        var mappedDocumentType = _mapper.Map<GetDocumentTypeByIdResponse>(documentType);
        return await Result<GetDocumentTypeByIdResponse>.SuccessAsync(mappedDocumentType);
    }
}
