using System.Net;
using System.Net.Http.Json;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Tests.Controllers;

/// <summary>
/// Integration tests for the CategoriesController.
/// </summary>
public class CategoriesControllerTests : IntegrationTestBase, IClassFixture<WebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CategoriesControllerTests"/> class.
    /// </summary>
    /// <param name="factory">The web application factory.</param>
    public CategoriesControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    /// <summary>
    /// Tests that GET /api/categories returns OK status code.
    /// </summary>
    [Fact]
    public async Task GetCategories_ReturnsOk()
    {
        // Arrange
        await SeedTestDataAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/categories");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /// <summary>
    /// Tests that POST /api/categories with valid data returns Created status code.
    /// </summary>
    [Fact]
    public async Task CreateCategory_WithValidData_ReturnsCreated()
    {
        // Arrange
        await SeedTestDataAsync();
        var category = new CreateCategoryDto
        {
            Name = "Test Category 2",
            Description = "Test Description 2"
        };
        
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/categories")
        {
            Content = JsonContent.Create(category)
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    /// <summary>
    /// Tests that POST /api/categories with invalid data returns BadRequest status code.
    /// </summary>
    [Fact]
    public async Task CreateCategory_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        await SeedTestDataAsync();
        var category = new CreateCategoryDto
        {
            Name = "", // Invalid: empty name
            Description = "Test Description"
        };
        
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/categories")
        {
            Content = JsonContent.Create(category)
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "token-1-admin@example.com-123456789");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}