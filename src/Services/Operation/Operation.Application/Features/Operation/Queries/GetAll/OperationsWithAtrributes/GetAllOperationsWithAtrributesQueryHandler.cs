using MediatR;
using Newtonsoft.Json;
using Operation.Application.Contracts.Services;
using Operation.Application.Features.Operation.Queries.GetAll.Attributes;
using Operation.Application.Features.Operation.Queries.GetAll.Operations;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;
using System.Dynamic;

namespace Operation.Application.Features.Operation.Queries.GetAll.OperationsWithAtrributes;

public class GetAllOperationsWithAtrributesQueryHandler : IRequestHandler<GetAllOperationsWithAtrributesQuery, Result<List<ExpandoObject>>>
{
    private readonly ICacheService _cache;
    private readonly ICosmosService _cosmosService;
    private readonly IMediator _mediator;

    public GetAllOperationsWithAtrributesQueryHandler(ICacheService cache, ICosmosService cosmosService, IMediator mediator)
    {
        _cache = cache;
        _cosmosService = cosmosService;
        _mediator = mediator;
    }
    public async Task<Result<List<ExpandoObject>>> Handle(GetAllOperationsWithAtrributesQuery request, CancellationToken cancellationToken)
    {
        var cacheData = await _cache.GetAsync<IEnumerable<ExpandoObject>>(ApplicationConstants.Cache.OPERATIONWITHATTRIBUTE_KEY);
        if (cacheData != null && cacheData.Any())
            return await Result<List<ExpandoObject>>.SuccessAsync(cacheData.ToList());

        var operations = (await _mediator.Send(new GetAllOperationsQuery(), cancellationToken)).Data;
        var attributes = (await _mediator.Send(new GetAllAttributesQuery(), cancellationToken)).Data.Select(x => (dynamic)x);

        var union = from x in operations
                    join z in attributes on x.Id.ToString() equals z.id into gj
                    from subZ in gj.DefaultIfEmpty()
                    select new { Operation = x, Atrributes = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(subZ)) };

        var result = union.Select(x => JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(x)));

        await _cache.SetAsync(ApplicationConstants.Cache.OPERATIONWITHATTRIBUTE_KEY, result, DateTimeOffset.Now.AddMinutes(30));

        return await Result<List<ExpandoObject>>.SuccessAsync(result.ToList());
    }
}
