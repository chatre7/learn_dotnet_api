using System.Net;
using System.Net.Http.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Tests.Controllers;

/// <summary>
/// Integration tests for the PostsController.
/// </summary>
public class PostsControllerTests : IntegrationTestBase, IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostsControllerTests"/> class.
    /// </summary>
    /// <param name="factory">The web application factory.</param>
    public PostsControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    /// <summary>
    /// Tests that GET /api/posts returns OK status code.
    /// </summary>
    [Fact]
    public async Task GetPosts_ReturnsOk()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/posts");
        request.Headers.Add("Authorization", "Bearer token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /// <summary>
    /// Tests that POST /api/posts with valid data returns Created status code.
    /// </summary>
    [Fact]
    public async Task CreatePost_WithValidData_ReturnsCreated()
    {
        // Arrange
        var post = new CreatePostDto
        {
            Title = "Test Post",
            Content = "Test Content",
            UserId = 1,
            CategoryId = 1
        };
        
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/posts")
        {
            Content = JsonContent.Create(post)
        };
        request.Headers.Add("Authorization", "Bearer token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    /// <summary>
    /// Tests that POST /api/posts with invalid data returns BadRequest status code.
    /// </summary>
    [Fact]
    public async Task CreatePost_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var post = new CreatePostDto
        {
            Title = "", // Invalid: empty title
            Content = "Test Content",
            UserId = 1,
            CategoryId = 1
        };
        
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/posts")
        {
            Content = JsonContent.Create(post)
        };
        request.Headers.Add("Authorization", "Bearer token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}