using AutoMapper;
using MediatR;
using Operation.Application.Common.Exceptions;
using Operation.Application.Contracts.Repositories;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Queries.GetById;


public class GetOperationByCodeQuery : IRequest<Result<GetOperationResponse>>
{
    public string Code { get; set; } = string.Empty;
}

internal class GetOperationByCodeQueryHandler : IRequestHandler<GetOperationByCodeQuery, Result<GetOperationResponse>>
{
    private readonly IMapper _mapper;
    private readonly IOperationRepository _operationRepository;

    public GetOperationByCodeQueryHandler(IMapper mapper, IOperationRepository operationRepository)
    {
        _mapper = mapper;
        _operationRepository = operationRepository;
    }

    public async Task<Result<GetOperationResponse>> Handle(GetOperationByCodeQuery query, CancellationToken cancellationToken)
    {
        var operation = await _operationRepository.GetByCode(query.Code) ?? throw new NotFoundException("Not found operation"); ;
        var mappedOperation = _mapper.Map<GetOperationResponse>(operation);
        return await Result<GetOperationResponse>.SuccessAsync(mappedOperation);
    }
}
