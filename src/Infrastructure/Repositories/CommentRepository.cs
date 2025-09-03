using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly NpgsqlConnection _connection;

    public CommentRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Comments WHERE Id = @Id";
        return await _connection.QueryFirstOrDefaultAsync<Comment>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Comment>> GetAllAsync()
    {
        var sql = "SELECT * FROM Comments";
        return await _connection.QueryAsync<Comment>(sql);
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        var sql = @"INSERT INTO Comments (Content, UserId, PostId, CreatedAt) 
                    VALUES (@Content, @UserId, @PostId, @CreatedAt) 
                    RETURNING *";
        
        comment.CreatedAt = DateTime.UtcNow;
        
        var createdComment = await _connection.QuerySingleAsync<Comment>(sql, comment);
        return createdComment;
    }

    public async Task<Comment> UpdateAsync(Comment comment)
    {
        // Comments typically aren't updated, but we'll implement it for completeness
        var sql = @"UPDATE Comments 
                    SET Content = @Content 
                    WHERE Id = @Id 
                    RETURNING *";
        
        var updatedComment = await _connection.QuerySingleAsync<Comment>(sql, comment);
        return updatedComment;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Comments WHERE Id = @Id";
        var result = await _connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}