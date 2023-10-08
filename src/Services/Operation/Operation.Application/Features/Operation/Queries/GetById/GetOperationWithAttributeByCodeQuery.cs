using MediatR;
using Newtonsoft.Json;
using Operation.Shared.Wrapper;
using System.Dynamic;

namespace Operation.Application.Features.Operation.Queries.GetById;

public record GetOperationWithAttributeByCodeQuery(string Code) : IRequest<Result<ExpandoObject>>;

internal class GetOperationWithAttributeByCodeQueryHandler : IRequestHandler<GetOperationWithAttributeByCodeQuery, Result<ExpandoObject>>
{
    private readonly IMediator _mediator;

    public GetOperationWithAttributeByCodeQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<ExpandoObject>> Handle(GetOperationWithAttributeByCodeQuery query, CancellationToken cancellationToken)
    {
        var operation = await _mediator.Send(new GetOperationByCodeQuery(query.Code), cancellationToken);
        var attribute = await _mediator.Send(new GetAttributeByCodeQuery(query.Code), cancellationToken);

        var result = new { Operation = operation, Attribute = attribute };

        return await Result<ExpandoObject>
                .SuccessAsync(JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(result)));
    }
}
