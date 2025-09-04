namespace Application.Interfaces;

/// <summary>
/// Interface for authentication service operations.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Validates user credentials.
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>True if credentials are valid; otherwise, false.</returns>
    bool ValidateCredentials(string email, string password);
    
    /// <summary>
    /// Generates a JWT token for the user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="email">The user's email.</param>
    /// <returns>A JWT token.</returns>
    string GenerateToken(int userId, string email);
}