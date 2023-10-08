using MediatR;
using Microsoft.Azure.Cosmos;
using Operation.Application.Common.Exceptions;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Wrapper;
using System.Dynamic;

namespace Operation.Application.Features.Operation.Queries.GetById;

public record GetAttributeByCodeQuery(string Code) : IRequest<Result<ExpandoObject>>;

internal class GetAttributeByCodeQueryHandler : IRequestHandler<GetAttributeByCodeQuery, Result<ExpandoObject>>
{
    private readonly IOperationService _operationService;
    private readonly IOperationRepository _operationRepository;

    public GetAttributeByCodeQueryHandler(IOperationService operationService, IOperationRepository operationRepository)
    {
        _operationService = operationService;
        _operationRepository = operationRepository;
    }

    public async Task<Result<ExpandoObject>> Handle(GetAttributeByCodeQuery query, CancellationToken cancellationToken)
    {
        var operation = await _operationRepository.GetByCode(query.Code) ?? throw new NotFoundException("Not found operation"); ;
        var attribute = await _operationService.GetAttribute(operation.Id.ToString(), new PartitionKey(operation.Code));
        return await Result<ExpandoObject>.SuccessAsync(attribute);
    }
}

