using System.Net;
using System.Text.Json;

namespace WebApi.Middleware;

/// <summary>
/// Middleware for handling exceptions globally in the application.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger instance.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware asynchronously.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles exceptions by returning appropriate HTTP responses.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="exception">The exception to handle.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = exception switch
        {
            ArgumentException => new ExceptionResponse(HttpStatusCode.BadRequest, "Invalid argument provided"),
            KeyNotFoundException => new ExceptionResponse(HttpStatusCode.NotFound, "Resource not found"),
            UnauthorizedAccessException => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized access"),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        context.Response.StatusCode = (int)response.StatusCode;
        var result = JsonSerializer.Serialize(new { error = response.Message });
        await context.Response.WriteAsync(result);
    }
}

/// <summary>
/// Represents a response for an exception.
/// </summary>
/// <param name="StatusCode">The HTTP status code.</param>
/// <param name="Message">The error message.</param>
public record ExceptionResponse(HttpStatusCode StatusCode, string Message);