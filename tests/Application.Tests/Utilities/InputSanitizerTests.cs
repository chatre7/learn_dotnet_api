using Application.Utilities;
using System.Diagnostics;

namespace Application.Tests.Utilities;

/// <summary>
/// Unit tests for the InputSanitizer utility.
/// </summary>
public class InputSanitizerTests
{
    /// <summary>
    /// Tests that SanitizeString removes HTML tags.
    /// </summary>
    [Fact]
    public void SanitizeString_RemovesHtmlTags()
    {
        // Arrange
        var input = "<script>alert('test');</script>Hello World";
        var expected = "Hello World";

        // Act
        var result = InputSanitizer.SanitizeString(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Tests that SanitizeString removes SQL injection patterns.
    /// </summary>
    [Fact]
    public void SanitizeString_RemovesSqlInjectionPatterns()
    {
        // Arrange
        var input = "SELECT * FROM users; DROP TABLE users;";
        var expected = " * FROM users;  TABLE users;";

        // Act
        var result = InputSanitizer.SanitizeString(input);

        // Debug output
        Debug.WriteLine($"Input: '{input}'");
        Debug.WriteLine($"Expected: '{expected}'");
        Debug.WriteLine($"Actual: '{result}'");
        Debug.WriteLine($"Expected length: {expected.Length}");
        Debug.WriteLine($"Actual length: {result.Length}");

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Tests that SanitizeEmail removes spaces and converts to lowercase.
    /// </summary>
    [Fact]
    public void SanitizeEmail_RemovesSpacesAndConvertsToLowercase()
    {
        // Arrange
        var input = " User@Example.Com ";
        var expected = "user@example.com";

        // Act
        var result = InputSanitizer.SanitizeEmail(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Tests that SanitizeString handles null input.
    /// </summary>
    [Fact]
    public void SanitizeString_HandlesNullInput()
    {
        // Arrange
        string? input = null;
        var expected = string.Empty;

        // Act
        var result = InputSanitizer.SanitizeString(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Tests that SanitizeEmail handles null input.
    /// </summary>
    [Fact]
    public void SanitizeEmail_HandlesNullInput()
    {
        // Arrange
        string? input = null;
        var expected = string.Empty;

        // Act
        var result = InputSanitizer.SanitizeEmail(input);

        // Assert
        Assert.Equal(expected, result);
    }
}