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
        
        // Use a transaction to ensure atomicity
        using var transaction = await connection.BeginTransactionAsync();
        
        try
        {
            // Clear existing data in correct order to avoid foreign key constraints
            await connection.ExecuteAsync("DELETE FROM Comments", transaction: transaction);
            await connection.ExecuteAsync("DELETE FROM Posts", transaction: transaction);
            await connection.ExecuteAsync("DELETE FROM Categories", transaction: transaction);
            await connection.ExecuteAsync("DELETE FROM Users", transaction: transaction);
            
            // Small delay to help with concurrency issues
            await Task.Delay(10);
            
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
            }, transaction: transaction);
            
            // Insert test category
            var categorySql = @"
                INSERT INTO Categories (Name, Description) 
                VALUES (@Name, @Description) 
                RETURNING Id";
            
            var categoryId = await connection.QuerySingleAsync<int>(categorySql, new 
            {
                Name = "Test Category",
                Description = "Test Description"
            }, transaction: transaction);
            
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
            }, transaction: transaction);
            
            // Commit the transaction
            await transaction.CommitAsync();
            
            return (userId, categoryId, postId);
        }
        catch
        {
            // Rollback the transaction in case of error
            await transaction.RollbackAsync();
            throw;
        }
    }
}