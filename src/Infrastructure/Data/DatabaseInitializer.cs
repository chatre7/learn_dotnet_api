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
}