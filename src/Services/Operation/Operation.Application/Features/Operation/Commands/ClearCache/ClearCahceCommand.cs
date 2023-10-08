using MediatR;
using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using Operation.Shared.Wrapper;

namespace Operation.Application.Features.Operation.Commands.ClearCache;

public record ClearCahceCommand : IRequest<Result>;

public class ClearCahceCommandHandler : IRequestHandler<ClearCahceCommand, Result>
{   
    private readonly ICacheService _cacheService;

    public ClearCahceCommandHandler(ICacheService cacheService)
    {     
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(ClearCahceCommand request, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(ApplicationConstants.Cache.OPERATIONATTRIBUTE_KEY);
        await _cacheService.RemoveAsync(ApplicationConstants.Cache.OPERATION_KEY);
        await _cacheService.RemoveAsync(ApplicationConstants.Cache.OPERATIONWITHATTRIBUTE_KEY);
        return (Result)await Result.SuccessAsync();
    }

}
