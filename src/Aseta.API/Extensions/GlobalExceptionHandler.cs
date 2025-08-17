using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Extensions;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        var statusCode = GetStatusCode(exception);

        var details = new ProblemDetails
        {
            Detail = exception.Message,
            Status = statusCode,
            Title = "API Exception",
            Type = exception.GetType().Name,
        };

        var response = JsonSerializer.Serialize(details);
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsync(response, cancellationToken);

        return true;
    }

    private static int GetStatusCode(Exception exception) => exception switch
    {
        UnauthorizedAccessException=>(int)HttpStatusCode.Unauthorized,
        _ => (int)HttpStatusCode.InternalServerError,
    };
}