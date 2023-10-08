using AutoMapper;
using MediatR;
using Operation.Application.Common.Exceptions;
using Operation.Application.Contracts.Repositories;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Queries.GetById;

public record GetOperationByIdQuery(int Id) : IRequest<Result<GetOperationResponse>>;

internal class GetOperationByIdQueryHandler : IRequestHandler<GetOperationByIdQuery, Result<GetOperationResponse>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IMapper _mapper;

    public GetOperationByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetOperationResponse>> Handle(GetOperationByIdQuery query, CancellationToken cancellationToken)
    {
        var operation = await _unitOfWork.Repository<Domain.Entities.Operation>().GetByIdAsync(query.Id) ?? throw new NotFoundException("Not found operation");
        var mappedOperation = _mapper.Map<GetOperationResponse>(operation);
        return await Result<GetOperationResponse>.SuccessAsync(mappedOperation);
    }
}

