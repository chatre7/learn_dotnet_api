using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        // Register database connection
        services.AddScoped<NpgsqlConnection>(sp => new NpgsqlConnection(connectionString));
        
        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<Domain.Interfaces.ICategoryRepository, Repositories.CategoryRepository>();
        services.AddScoped<Domain.Interfaces.ICommentRepository, Repositories.CommentRepository>();
        
        return services;
    }
}