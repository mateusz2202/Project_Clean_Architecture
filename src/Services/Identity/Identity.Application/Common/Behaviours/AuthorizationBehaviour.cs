﻿using Identity.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<TRequest> _logger;
    private readonly IHttpContextAccessor _accessor;

    public AuthorizationBehaviour(ICurrentUserService currentUserService, ILogger<TRequest> logger, IHttpContextAccessor accessor)
    {
        _currentUserService = currentUserService;
        _logger = logger;
        _accessor = accessor;
    }   

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            var d = _accessor.HttpContext.Request.Path.ToString();
            var userId = _currentUserService.User?.Identity;
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex, "Request: Unauthorized for request {Name} {@Request} {UserId}", requestName, request, _currentUserService.User?.Identity);

            throw;
        }
    }
}