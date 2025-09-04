using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Tests.Controllers;

/// <summary>
/// Integration tests for the AuthController.
/// </summary>
public class AuthControllerTests : IntegrationTestBase, IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthControllerTests"/> class.
    /// </summary>
    /// <param name="factory">The web application factory.</param>
    public AuthControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    /// <summary>
    /// Tests that POST /api/auth/login with valid credentials returns OK status code.
    /// </summary>
    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOk()
    {
        // Arrange
        var loginDto = new
        {
            Email = "admin@example.com",
            Password = "admin123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /// <summary>
    /// Tests that POST /api/auth/login with invalid credentials returns Unauthorized status code.
    /// </summary>
    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new
        {
            Email = "invalid@example.com",
            Password = "wrongpassword"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}