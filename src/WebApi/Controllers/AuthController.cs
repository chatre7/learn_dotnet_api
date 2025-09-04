using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers;

/// <summary>
/// Controller for handling authentication operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="authenticationService">The authentication service.</param>
    /// <param name="logger">The logger instance.</param>
    public AuthController(IAuthenticationService authenticationService, ILogger<AuthController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a user and returns a token.
    /// </summary>
    /// <param name="loginDto">The login credentials.</param>
    /// <returns>An authentication token if successful; otherwise, Unauthorized.</returns>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for login");
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Attempting login for email: {Email}", loginDto.Email);
        
        // For demonstration purposes, we're using a fixed user ID
        // In a real application, you would retrieve the user from the database
        if (_authenticationService.ValidateCredentials(loginDto.Email, loginDto.Password))
        {
            var token = _authenticationService.GenerateToken(1, loginDto.Email);
            _logger.LogInformation("Login successful for email: {Email}", loginDto.Email);
            return Ok(new { Token = token });
        }

        _logger.LogWarning("Login failed for email: {Email}", loginDto.Email);
        return Unauthorized(new { Message = "Invalid credentials" });
    }
}

/// <summary>
/// Data transfer object for login credentials.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Gets or sets the user's email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}