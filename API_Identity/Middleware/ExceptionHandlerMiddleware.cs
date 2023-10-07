using API_Identity.Exceptions;
using System.Net;

namespace API_Identity.Middleware;

public class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await ConvertException(context, ex);
        }
    }

    private Task ConvertException(HttpContext context, Exception exception)
    {
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

        switch (exception)
        {
            case BadRequestException:
                _logger.LogError(exception, exception.Message);
                httpStatusCode = HttpStatusCode.BadRequest;
                break;
            case ForbidException:
                _logger.LogError(exception, exception.Message);
                httpStatusCode = HttpStatusCode.Forbidden;
                break;
            case NotFoundException:
                _logger.LogInformation(exception, exception.Message);
                httpStatusCode = HttpStatusCode.NotFound;
                break;
            case Exception:
                _logger.LogError(exception, exception.Message);
                httpStatusCode = HttpStatusCode.InternalServerError;
                break;
        }

        context.Response.StatusCode = (int)httpStatusCode;

        return context.Response.WriteAsync(exception.Message);
    }


}
