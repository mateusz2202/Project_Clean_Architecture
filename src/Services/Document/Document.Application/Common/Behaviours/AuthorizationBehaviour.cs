using Document.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Document.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<TRequest> _logger;

    public AuthorizationBehaviour(ICurrentUserService currentUserService, ILogger<TRequest> logger)
    {
        _currentUserService = currentUserService;
        _logger = logger;      
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {          
            var userId = _currentUserService.UserId;
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex, "Request: Unauthorized for request {Name} {@Request} {UserId}", requestName, request, _currentUserService.UserId);

            throw;
        }
    }
}
