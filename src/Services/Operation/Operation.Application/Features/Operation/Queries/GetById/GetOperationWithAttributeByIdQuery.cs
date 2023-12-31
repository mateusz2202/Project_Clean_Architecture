﻿using MediatR;
using Newtonsoft.Json;
using Operation.Shared.Wrapper;
using System.Dynamic;

namespace Operation.Application.Features.Operation.Queries.GetById;

public record GetOperationWithAttributeByIdQuery(int Id) : IRequest<Result<ExpandoObject>>;

internal class GetOperationWithAttributeByIdQueryHandler : IRequestHandler<GetOperationWithAttributeByIdQuery, Result<ExpandoObject>>
{   
    private readonly IMediator _mediator;

    public GetOperationWithAttributeByIdQueryHandler(IMediator mediator)
    {      
        _mediator = mediator;
    }

    public async Task<Result<ExpandoObject>> Handle(GetOperationWithAttributeByIdQuery query, CancellationToken cancellationToken)
    {
        var operation = await _mediator.Send(new GetOperationByIdQuery(query.Id), cancellationToken);
        var attribute = await _mediator.Send(new GetAttributeByIddQuery(query.Id), cancellationToken);

        var result = new { Operation = operation, Attribute = attribute };

        return await Result<ExpandoObject>
                .SuccessAsync(JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(result)));
    }
}