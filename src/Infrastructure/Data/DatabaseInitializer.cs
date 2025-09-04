using Dapper;
using Npgsql;

namespace Infrastructure.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(string connectionString)
    {
        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        // Create Users table
        var createUsersTableSql = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id SERIAL PRIMARY KEY,
                Name VARCHAR(100) NOT NULL,
                Email VARCHAR(255) UNIQUE NOT NULL,
                CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
                UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
            )";

        await connection.ExecuteAsync(createUsersTableSql);

        // Create Categories table
        var createCategoriesTableSql = @"
            CREATE TABLE IF NOT EXISTS Categories (
                Id SERIAL PRIMARY KEY,
                Name VARCHAR(100) NOT NULL,
                Description TEXT
            )";

        await connection.ExecuteAsync(createCategoriesTableSql);

        // Create Posts table
        var createPostsTableSql = @"
            CREATE TABLE IF NOT EXISTS Posts (
                Id SERIAL PRIMARY KEY,
                Title VARCHAR(200) NOT NULL,
                Content TEXT NOT NULL,
                UserId INTEGER NOT NULL,
                CategoryId INTEGER NOT NULL,
                CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
                UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
                FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
                FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE CASCADE
            )";

        await connection.ExecuteAsync(createPostsTableSql);

        // Create Comments table
        var createCommentsTableSql = @"
            CREATE TABLE IF NOT EXISTS Comments (
                Id SERIAL PRIMARY KEY,
                Content TEXT NOT NULL,
                UserId INTEGER NOT NULL,
                PostId INTEGER NOT NULL,
                CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
                FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
                FOREIGN KEY (PostId) REFERENCES Posts(Id) ON DELETE CASCADE
            )";

        await connection.ExecuteAsync(createCommentsTableSql);
    }
    
    /// <summary>
    /// Seeds the database with test data for integration tests.
    /// </summary>
    /// <param name="connectionString">The database connection string.</param>
    public static async Task<(int UserId, int CategoryId, int PostId)> SeedTestDataAsync(string connectionString)
    {
        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        // Check if test data already exists
        var userExists = await connection.QuerySingleOrDefaultAsync<int?>(
            "SELECT Id FROM Users WHERE Email = @Email", 
            new { Email = "test@example.com" });
        
        if (userExists.HasValue)
        {
            // Data already exists, return the existing IDs
            var existingUserId = userExists.Value;
            var existingCategoryId = await connection.QuerySingleAsync<int>(
                "SELECT Id FROM Categories WHERE Name = @Name", 
                new { Name = "Test Category" });
            var existingPostId = await connection.QuerySingleAsync<int>(
                "SELECT Id FROM Posts WHERE Title = @Title AND UserId = @UserId", 
                new { Title = "Test Post", UserId = existingUserId });
            
            return (existingUserId, existingCategoryId, existingPostId);
        }
        
        // Clear existing data
        await connection.ExecuteAsync("DELETE FROM Comments");
        await connection.ExecuteAsync("DELETE FROM Posts");
        await connection.ExecuteAsync("DELETE FROM Categories");
        await connection.ExecuteAsync("DELETE FROM Users");
        
        // Insert test user
        var userSql = @"
            INSERT INTO Users (Name, Email, CreatedAt, UpdatedAt) 
            VALUES (@Name, @Email, @CreatedAt, @UpdatedAt) 
            RETURNING Id";
        
        var userId = await connection.QuerySingleAsync<int>(userSql, new 
        {
            Name = "Test User",
            Email = "test@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
        
        // Insert test category
        var categorySql = @"
            INSERT INTO Categories (Name, Description) 
            VALUES (@Name, @Description) 
            RETURNING Id";
        
        var categoryId = await connection.QuerySingleAsync<int>(categorySql, new 
        {
            Name = "Test Category",
            Description = "Test Description"
        });
        
        // Insert test post
        var postSql = @"
            INSERT INTO Posts (Title, Content, UserId, CategoryId, CreatedAt, UpdatedAt) 
            VALUES (@Title, @Content, @UserId, @CategoryId, @CreatedAt, @UpdatedAt) 
            RETURNING Id";
        
        var postId = await connection.QuerySingleAsync<int>(postSql, new 
        {
            Title = "Test Post",
            Content = "Test Content",
            UserId = userId,
            CategoryId = categoryId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
        
        return (userId, categoryId, postId);
    }
}