using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly NpgsqlConnection _connection;

    public PostRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Posts WHERE Id = @Id";
        return await _connection.QueryFirstOrDefaultAsync<Post>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        var sql = "SELECT * FROM Posts";
        return await _connection.QueryAsync<Post>(sql);
    }

    public async Task<Post> CreateAsync(Post post)
    {
        var sql = @"INSERT INTO Posts (Title, Content, UserId, CategoryId, CreatedAt, UpdatedAt) 
                    VALUES (@Title, @Content, @UserId, @CategoryId, @CreatedAt, @UpdatedAt) 
                    RETURNING *";
        
        post.CreatedAt = DateTime.UtcNow;
        post.UpdatedAt = DateTime.UtcNow;
        
        var createdPost = await _connection.QuerySingleAsync<Post>(sql, post);
        return createdPost;
    }

    public async Task<Post> UpdateAsync(Post post)
    {
        var sql = @"UPDATE Posts 
                    SET Title = @Title, Content = @Content, CategoryId = @CategoryId, UpdatedAt = @UpdatedAt 
                    WHERE Id = @Id 
                    RETURNING *";
        
        post.UpdatedAt = DateTime.UtcNow;
        
        var updatedPost = await _connection.QuerySingleAsync<Post>(sql, post);
        return updatedPost;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Posts WHERE Id = @Id";
        var result = await _connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}