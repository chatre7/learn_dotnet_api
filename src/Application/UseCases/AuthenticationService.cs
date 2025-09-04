using Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Application.UseCases;

/// <summary>
/// Service for handling authentication operations.
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly ILogger<AuthenticationService> _logger;
    private readonly string _secretKey = "your-secret-key-change-this-in-production"; // In production, use a secure secret key

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public AuthenticationService(ILogger<AuthenticationService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Validates user credentials.
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>True if credentials are valid; otherwise, false.</returns>
    public bool ValidateCredentials(string email, string password)
    {
        _logger.LogInformation("Validating credentials for email: {Email}", email);
        
        // In a real application, you would check against a database
        // For this example, we'll use a simple check
        var hashedPassword = HashPassword(password);
        
        // This is just for demonstration - in a real app, you'd query the database
        var isValid = email == "admin@example.com" && hashedPassword == HashPassword("admin123");
        
        if (isValid)
        {
            _logger.LogInformation("Credentials validated successfully for email: {Email}", email);
        }
        else
        {
            _logger.LogWarning("Invalid credentials for email: {Email}", email);
        }
        
        return isValid;
    }

    /// <summary>
    /// Generates a JWT token for the user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="email">The user's email.</param>
    /// <returns>A JWT token.</returns>
    public string GenerateToken(int userId, string email)
    {
        _logger.LogInformation("Generating token for user ID: {UserId}, email: {Email}", userId, email);
        
        // In a real application, you would generate a proper JWT token
        // For this example, we'll create a simple token representation
        var token = $"token-{userId}-{email}-{DateTime.UtcNow.Ticks}";
        
        _logger.LogInformation("Token generated successfully for user ID: {UserId}", userId);
        return token;
    }

    /// <summary>
    /// Hashes a password using SHA256.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password.</returns>
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}