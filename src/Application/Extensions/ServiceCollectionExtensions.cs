using Application.Interfaces;
using Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

/// <summary>
/// Extension methods for configuring application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds application services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICommentService, CommentService>();
        
        return services;
    }
}