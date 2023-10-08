using MediatR;
using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;
using System.Dynamic;

namespace Operation.Application.Features.Operation.Queries.GetAll.Attributes;
public record GetAllAttributesQuery : IRequest<Result<List<ExpandoObject>>>;

public class GetAllAttributesQueryHandler : IRequestHandler<GetAllAttributesQuery, Result<List<ExpandoObject>>>
{
    private readonly ICacheService _cache;
    private readonly ICosmosService _cosmosService;

    public GetAllAttributesQueryHandler(ICacheService cache, ICosmosService cosmosService)
    {
        _cache = cache;
        _cosmosService = cosmosService;
    }
    public async Task<Result<List<ExpandoObject>>> Handle(GetAllAttributesQuery request, CancellationToken cancellationToken)
    {
        var cacheData = await _cache.GetAsync<IEnumerable<ExpandoObject>>(ApplicationConstants.Cache.OPERATIONATTRIBUTE_KEY);

        if (cacheData != null && cacheData.Any())
            return await Result<List<ExpandoObject>>.SuccessAsync(cacheData.ToList());

        var result = await _cosmosService.GetAll(ApplicationConstants.CosmosDB.CONTAINER_OPERATION, cancellationToken);

        await _cache.SetAsync(ApplicationConstants.Cache.OPERATIONATTRIBUTE_KEY, result, DateTimeOffset.Now.AddMinutes(30));

        return await Result<List<ExpandoObject>>.SuccessAsync(result);
    }
}
