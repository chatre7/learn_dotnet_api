using System.Text.RegularExpressions;

namespace Application.Utilities;

/// <summary>
/// Utility class for sanitizing user inputs.
/// </summary>
public static class InputSanitizer
{
    /// <summary>
    /// Sanitizes a string by removing potentially dangerous characters.
    /// </summary>
    /// <param name="input">The input string to sanitize.</param>
    /// <returns>The sanitized string.</returns>
    public static string SanitizeString(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Remove HTML tags
        var sanitized = Regex.Replace(input, @"<[^>]*>", string.Empty);
        
        // Remove script tags and JavaScript
        sanitized = Regex.Replace(sanitized, @"(?i)<script[^>]*>.*?</script>", string.Empty, RegexOptions.Singleline);
        
        // Remove SQL injection patterns
        sanitized = Regex.Replace(sanitized, @"(?i)(union|select|insert|update|delete|drop|create|alter|exec|execute)", string.Empty, RegexOptions.IgnoreCase);
        
        // Limit length to prevent overflow
        if (sanitized.Length > 1000)
            sanitized = sanitized.Substring(0, 1000);
            
        return sanitized.Trim();
    }

    /// <summary>
    /// Sanitizes an email address.
    /// </summary>
    /// <param name="email">The email address to sanitize.</param>
    /// <returns>The sanitized email address.</returns>
    public static string SanitizeEmail(string? email)
    {
        if (string.IsNullOrEmpty(email))
            return string.Empty;

        // Remove any HTML tags
        var sanitized = Regex.Replace(email, @"<[^>]*>", string.Empty);
        
        // Remove any spaces
        sanitized = sanitized.Replace(" ", "");
        
        // Limit length
        if (sanitized.Length > 255)
            sanitized = sanitized.Substring(0, 255);
            
        return sanitized.ToLowerInvariant().Trim();
    }
}