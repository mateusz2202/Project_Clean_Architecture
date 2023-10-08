using MediatR;
using Microsoft.Azure.Cosmos;
using Operation.Application.Common.Exceptions;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;
using System.Dynamic;

namespace Operation.Application.Features.Operation.Queries.GetById;

public record GetAttributeByCodeQuery(string Code) : IRequest<Result<ExpandoObject>>;

internal class GetAttributeByCodeQueryHandler : IRequestHandler<GetAttributeByCodeQuery, Result<ExpandoObject>>
{
    private readonly ICosmosService _cosmosService;
    private readonly IOperationRepository _operationRepository;

    public GetAttributeByCodeQueryHandler(IOperationRepository operationRepository, ICosmosService cosmosService)
    {
        _operationRepository = operationRepository;
        _cosmosService = cosmosService;
    }

    public async Task<Result<ExpandoObject>> Handle(GetAttributeByCodeQuery query, CancellationToken cancellationToken)
    {
        var operation = await _operationRepository
            .GetByCode(query.Code)
            ?? throw new NotFoundException("Not found operation"); ;

        var attribute = await _cosmosService
            .Get(
            containerName: ApplicationConstants.CosmosDB.CONTAINER_OPERATION,
            id: operation.Id.ToString(),
            partitionKey: new PartitionKey(operation.Code),
            cancellationToken: cancellationToken);

        return await Result<ExpandoObject>.SuccessAsync(attribute);
    }
}

