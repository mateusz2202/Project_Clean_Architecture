using MediatR;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Operation.Application.Contracts.Services;
using Operation.Application.Features.Operation.Commands.AddOperation;
using Operation.Application.Features.Operation.Queries.GetById;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.AddOperationWithAtribute;

public record AddOperationWithAttributCommad(AddOperationCommand AddOperationCommand, object Attributes) : IRequest<Result>;

public class AddOperationWithAttributCommadHandler : IRequestHandler<AddOperationWithAttributCommad, Result>
{
    private readonly ICosmosService _cosmosService;
    private readonly IMediator _mediator;
    public AddOperationWithAttributCommadHandler(
        IMediator mediator,
        ICosmosService cosmosService)
    {
        _mediator = mediator;
        _cosmosService = cosmosService;
    }

    public async Task<Result> Handle(AddOperationWithAttributCommad request, CancellationToken cancellationToken)
    {
        var addObj = await _mediator.Send(request.AddOperationCommand, cancellationToken);
        
        var operation = (await _mediator.Send(new GetOperationByIdQuery(addObj.Data), cancellationToken)).Data;
        
        var item = JsonConvert.DeserializeObject<dynamic>(request.Attributes.ToString());
        
        ((dynamic)item).id = operation.Id.ToString();
        ((dynamic)item).code = operation.Code;

        string json = JsonConvert.SerializeObject(item);

        await _cosmosService
                .AddOrEdit(
                        containerName: ApplicationConstants.CosmosDB.CONTAINER_OPERATION,
                        id: operation.Id.ToString(),
                        partitionKey: new PartitionKey(operation.Code),
                        item: json,
                        cancellationToken: cancellationToken);

        return await Result<int>.SuccessAsync();
    }

}