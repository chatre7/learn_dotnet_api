using System.Net;
using System.Net.Http.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Tests.Controllers;

/// <summary>
/// Integration tests for the CommentsController.
/// </summary>
public class CommentsControllerTests : IntegrationTestBase, IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommentsControllerTests"/> class.
    /// </summary>
    /// <param name="factory">The web application factory.</param>
    public CommentsControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    /// <summary>
    /// Tests that GET /api/comments returns OK status code.
    /// </summary>
    [Fact]
    public async Task GetComments_ReturnsOk()
    {
        // Arrange
        await SeedTestDataAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/comments");
        request.Headers.Add("Authorization", "Bearer token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /// <summary>
    /// Tests that POST /api/comments with valid data returns Created status code.
    /// </summary>
    [Fact]
    public async Task CreateComment_WithValidData_ReturnsCreated()
    {
        // Arrange
        await SeedTestDataAsync();
        var comment = new CreateCommentDto
        {
            Content = "Test Comment",
            UserId = _userId, // Use the correct user ID from seeded data
            PostId = _postId  // Use the correct post ID from seeded data
        };
        
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/comments")
        {
            Content = JsonContent.Create(comment)
        };
        request.Headers.Add("Authorization", "Bearer token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    /// <summary>
    /// Tests that POST /api/comments with invalid data returns BadRequest status code.
    /// </summary>
    [Fact]
    public async Task CreateComment_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        await SeedTestDataAsync();
        var comment = new CreateCommentDto
        {
            Content = "", // Invalid: empty content
            UserId = _userId,   // Use the correct user ID from seeded data
            PostId = _postId    // Use the correct post ID from seeded data
        };
        
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/comments")
        {
            Content = JsonContent.Create(comment)
        };
        request.Headers.Add("Authorization", "Bearer token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}