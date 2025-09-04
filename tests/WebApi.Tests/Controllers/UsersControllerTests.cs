using System.Net;
using System.Net.Http.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Tests.Controllers;

/// <summary>
/// Integration tests for the UsersController.
/// </summary>
public class UsersControllerTests : IntegrationTestBase, IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UsersControllerTests"/> class.
    /// </summary>
    /// <param name="factory">The web application factory.</param>
    public UsersControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    /// <summary>
    /// Tests that GET /api/users returns OK status code.
    /// </summary>
    [Fact]
    public async Task GetUsers_ReturnsOk()
    {
        // Arrange
        await SeedTestDataAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/users");
        request.Headers.Add("Authorization", "Bearer token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /// <summary>
    /// Tests that POST /api/users with valid data returns Created status code.
    /// </summary>
    [Fact]
    public async Task CreateUser_WithValidData_ReturnsCreated()
    {
        // Arrange
        await SeedTestDataAsync();
        var user = new CreateUserDto
        {
            Name = "Test User 2",
            Email = "test2@example.com"
        };
        
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/users")
        {
            Content = JsonContent.Create(user)
        };
        request.Headers.Add("Authorization", "Bearer token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    /// <summary>
    /// Tests that POST /api/users with invalid data returns BadRequest status code.
    /// </summary>
    [Fact]
    public async Task CreateUser_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        await SeedTestDataAsync();
        var user = new CreateUserDto
        {
            Name = "", // Invalid: empty name
            Email = "invalid-email" // Invalid: malformed email
        };
        
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/users")
        {
            Content = JsonContent.Create(user)
        };
        request.Headers.Add("Authorization", "Bearer token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}