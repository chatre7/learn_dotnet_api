using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Tests.Controllers;

/// <summary>
/// Simple integration tests to verify authorization header handling.
/// </summary>
public class TestAuthorizationControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestAuthorizationControllerTests"/> class.
    /// </summary>
    /// <param name="factory">The web application factory.</param>
    public TestAuthorizationControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Tests that requests without authorization header return Unauthorized status code.
    /// </summary>
    [Fact]
    public async Task Request_WithoutAuthorizationHeader_ReturnsUnauthorized()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/categories");

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
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/categories");
        request.Headers.Add("Authorization", "Bearer token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}