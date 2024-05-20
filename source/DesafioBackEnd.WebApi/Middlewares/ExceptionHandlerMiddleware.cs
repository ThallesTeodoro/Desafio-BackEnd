using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.WebApi.Contracts;

namespace DesafioBackEnd.WebApi.Middlewares;

internal sealed class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandlerExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandlerExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var response = GetStatusCodeAndResponse(exception);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = response.StatusCode;

        _logger.LogError(
            "Request failure {@Error}, {@DateTimeUtc}",
            exception.Message,
            DateTime.UtcNow);

        await httpContext.Response.WriteAsync(response.ToString());
    }

    private JsonResponse<object, object> GetStatusCodeAndResponse(Exception exception)
        => exception switch
        {
            NotFoundException => new JsonResponse<object, object>(StatusCodes.Status404NotFound, null, null),
            ForbiddenException => new JsonResponse<object, object>(StatusCodes.Status403Forbidden, null, null),
            UnauthorizedException => new JsonResponse<object, object>(StatusCodes.Status401Unauthorized, null, null),
            ValidationException validationException => new JsonResponse<object, object>(StatusCodes.Status400BadRequest, null, validationException.Errors),
            _ => new JsonResponse<object, object>(StatusCodes.Status500InternalServerError, null, null)
        };
}
