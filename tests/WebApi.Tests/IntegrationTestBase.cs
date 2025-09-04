using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Infrastructure.Data;

namespace WebApi.Tests;

/// <summary>
/// Base class for integration tests.
/// </summary>
public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;
    protected int _userId;
    protected int _categoryId;
    protected int _postId;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationTestBase"/> class.
    /// </summary>
    /// <param name="factory">The web application factory.</param>
    public IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Override services for testing if needed
            });
        });
        
        _client = _factory.CreateClient();
    }
    
    /// <summary>
    /// Seeds the database with test data.
    /// </summary>
    protected async Task SeedTestDataAsync()
    {
        var connectionString = "Host=localhost;Port=5432;Database=blogdb;Username=postgres;Password=postgres";
        var (userId, categoryId, postId) = await DatabaseInitializer.SeedTestDataAsync(connectionString);
        _userId = userId;
        _categoryId = categoryId;
        _postId = postId;
    }
}