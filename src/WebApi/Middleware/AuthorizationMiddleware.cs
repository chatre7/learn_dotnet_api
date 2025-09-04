using Microsoft.Extensions.Logging;

namespace WebApi.Middleware;

/// <summary>
/// Middleware for handling authorization.
/// </summary>
public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger instance.</param>
    public AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
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
        // Skip authorization for the auth controller
        if (context.Request.Path.StartsWithSegments("/api/auth"))
        {
            await _next(context);
            return;
        }

        // Check for authorization header
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            _logger.LogWarning("Authorization header missing for request {Method} {Path}", context.Request.Method, context.Request.Path);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("{\"error\": \"Authorization header missing\"}");
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            _logger.LogWarning("Invalid authorization header for request {Method} {Path}", context.Request.Method, context.Request.Path);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("{\"error\": \"Invalid authorization header\"}");
            return;
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();
        if (!ValidateToken(token))
        {
            _logger.LogWarning("Invalid token for request {Method} {Path}", context.Request.Method, context.Request.Path);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("{\"error\": \"Invalid token\"}");
            return;
        }

        _logger.LogInformation("Authorization successful for request {Method} {Path}", context.Request.Method, context.Request.Path);
        await _next(context);
    }

    /// <summary>
    /// Validates a token.
    /// </summary>
    /// <param name="token">The token to validate.</param>
    /// <returns>True if the token is valid; otherwise, false.</returns>
    private static bool ValidateToken(string token)
    {
        // In a real application, you would validate the JWT token
        // For this example, we'll just check if it starts with "token-"
        return !string.IsNullOrWhiteSpace(token) && token.StartsWith("token-");
    }
}