using AutoMapper;
using MediatR;
using Operation.Application.Contracts.Repositories;
using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Queries.GetAll.Operations;

public record GetAllOperationsResponse(int Id,string Name,string Code);
public record GetAllOperationsQuery : IRequest<Result<List<GetAllOperationsResponse>>>;

public class GetAllOperationsQueryHandler : IRequestHandler<GetAllOperationsQuery, Result<List<GetAllOperationsResponse>>>
{
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public GetAllOperationsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result<List<GetAllOperationsResponse>>> Handle(GetAllOperationsQuery request, CancellationToken cancellationToken)
    {
        var cacheData = await _cache.GetAsync<IEnumerable<Domain.Entities.Operation>>(ApplicationConstants.Cache.OPERATION_KEY);

        if (cacheData != null && cacheData.Any())
            return await Result<List<GetAllOperationsResponse>>
                            .SuccessAsync(_mapper.Map<List<GetAllOperationsResponse>>(cacheData));

        var result = await _unitOfWork.Repository<Domain.Entities.Operation>().GetAllAsync();

        await _cache.SetAsync(ApplicationConstants.Cache.OPERATION_KEY, result, DateTimeOffset.Now.AddMinutes(30));

        var mappedOperations = _mapper.Map<List<GetAllOperationsResponse>>(result);
        return await Result<List<GetAllOperationsResponse>>.SuccessAsync(mappedOperations);
    }
}
