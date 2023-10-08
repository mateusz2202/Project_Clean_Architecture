using MediatR;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Operation.Application.Contracts.Services;
using Operation.Application.Features.Operation.Commands.AddOperation;
using Operation.Application.Features.Operation.Queries.GetById;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.AddOperationWithAtribute;

public record AddOperationWithAttributCommad(AddOperationCommand AddOperationCommand, object Attributes) : IRequest<Result>;

public class AddOperationWithAttributCommadHandler : IRequestHandler<AddOperationWithAttributCommad, Result>
{
    private readonly IOperationService _operationService;
    private readonly IMediator _mediator;
    public AddOperationWithAttributCommadHandler(
        IOperationService operationService,
        IMediator mediator)
    {
        _operationService = operationService;
        _mediator = mediator;
    }

    public async Task<Result> Handle(AddOperationWithAttributCommad request, CancellationToken cancellationToken)
    {

        var addObj = await _mediator.Send(request.AddOperationCommand, cancellationToken);
        var operation = (await _mediator.Send(new GetOperationByIdQuery(addObj.Data), cancellationToken)).Data;
        var item = JsonConvert.DeserializeObject<dynamic>(request.Attributes.ToString());
        ((dynamic)item).id = operation.Id.ToString();
        ((dynamic)item).code = operation.Code;

        string json = JsonConvert.SerializeObject(item);
        await _operationService.AddOrEditAtributes(operation.Id.ToString(), new PartitionKey(operation.Code), json, cancellationToken);

        return await Result<int>.SuccessAsync();
    }

}