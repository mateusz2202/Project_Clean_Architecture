using MediatR;
using Microsoft.Azure.Cosmos;
using Operation.Application.Common.Exceptions;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;
using System.Dynamic;

namespace Operation.Application.Features.Operation.Queries.GetById;

public record GetAttributeByIddQuery(int Id) : IRequest<Result<ExpandoObject>>;

internal class GetAttributeByIdHandler : IRequestHandler<GetAttributeByIddQuery, Result<ExpandoObject>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly ICosmosService _cosmosService;

    public GetAttributeByIdHandler(IUnitOfWork<int> unitOfWork, ICosmosService cosmosService)
    {
        _unitOfWork = unitOfWork;
        _cosmosService = cosmosService;
    }

    public async Task<Result<ExpandoObject>> Handle(GetAttributeByIddQuery query, CancellationToken cancellationToken)
    {
        var operation = await _unitOfWork
            .Repository<Domain.Entities.Operation>()
            .GetByIdAsync(query.Id)
            ?? throw new NotFoundException("Not found operation"); 

        var attribute = await _cosmosService
            .Get(
                containerName: ApplicationConstants.CosmosDB.CONTAINER_OPERATION,
                id: operation.Id.ToString(),
                partitionKey: new PartitionKey(operation.Code),
                cancellationToken: cancellationToken);

        return await Result<ExpandoObject>.SuccessAsync(attribute);
    }
}
