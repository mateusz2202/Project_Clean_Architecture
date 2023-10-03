using API_Identity.Exceptions;

namespace API_Identity.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (BadRequestException e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(e.Message);
        }
        catch (CreateResourceException e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(e.Message);
        }
        catch (ForbidException e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(e.Message);
        }
        catch (NotFoundException e)
        {
            _logger.LogInformation(e, e.Message);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Error, something went wrong");
        }
    }
}
