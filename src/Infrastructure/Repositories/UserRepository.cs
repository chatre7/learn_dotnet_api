using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NpgsqlConnection _connection;

    public UserRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Users WHERE Id = @Id";
        return await _connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var sql = "SELECT * FROM Users";
        return await _connection.QueryAsync<User>(sql);
    }

    public async Task<User> CreateAsync(User user)
    {
        var sql = @"INSERT INTO Users (Name, Email, CreatedAt, UpdatedAt) 
                    VALUES (@Name, @Email, @CreatedAt, @UpdatedAt) 
                    RETURNING *";
        
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        
        var createdUser = await _connection.QuerySingleAsync<User>(sql, user);
        return createdUser;
    }

    public async Task<User> UpdateAsync(User user)
    {
        var sql = @"UPDATE Users 
                    SET Name = @Name, Email = @Email, UpdatedAt = @UpdatedAt 
                    WHERE Id = @Id 
                    RETURNING *";
        
        user.UpdatedAt = DateTime.UtcNow;
        
        var updatedUser = await _connection.QuerySingleAsync<User>(sql, user);
        return updatedUser;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Users WHERE Id = @Id";
        var result = await _connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}