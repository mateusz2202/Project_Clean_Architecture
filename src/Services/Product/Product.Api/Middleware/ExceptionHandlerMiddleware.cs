using Product.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace Product.Api.Middleware;

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

        var result = string.Empty;

        switch (exception)
        {
            case ValidationException validationException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(validationException.Errors);
                break;
            case BadRequestException:
                _logger.LogError(exception, exception.Message);
                httpStatusCode = HttpStatusCode.BadRequest;
                break;
            case ForbiddenAccessException:
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

        if (result == string.Empty)
        {
            result = JsonSerializer.Serialize(new { error = exception.Message });
        }

        context.Response.StatusCode = (int)httpStatusCode;

        return context.Response.WriteAsync(result);
    }


}
