using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Tests.Middleware;

/// <summary>
/// Integration tests for the AuthorizationMiddleware.
/// </summary>
public class AuthorizationMiddlewareTests : IntegrationTestBase, IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationMiddlewareTests"/> class.
    /// </summary>
    /// <param name="factory">The web application factory.</param>
    public AuthorizationMiddlewareTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    /// <summary>
    /// Tests that requests without authorization header return Unauthorized status code.
    /// </summary>
    [Fact]
    public async Task Request_WithoutAuthorizationHeader_ReturnsUnauthorized()
    {
        // Arrange
        await SeedTestDataAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/categories");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    /// <summary>
    /// Tests that requests with invalid authorization header return Unauthorized status code.
    /// </summary>
    [Fact]
    public async Task Request_WithInvalidAuthorizationHeader_ReturnsUnauthorized()
    {
        // Arrange
        await SeedTestDataAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/categories");
        request.Headers.Add("Authorization", "InvalidToken");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    /// <summary>
    /// Tests that requests with valid authorization header return OK status code.
    /// </summary>
    [Fact]
    public async Task Request_WithValidAuthorizationHeader_ReturnsOk()
    {
        // Arrange
        await SeedTestDataAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/categories");
        request.Headers.Add("Authorization", "Bearer token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}