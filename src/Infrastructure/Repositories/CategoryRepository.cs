using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly NpgsqlConnection _connection;

    public CategoryRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Categories WHERE Id = @Id";
        return await _connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var sql = "SELECT * FROM Categories";
        return await _connection.QueryAsync<Category>(sql);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        var sql = @"INSERT INTO Categories (Name, Description) 
                    VALUES (@Name, @Description) 
                    RETURNING *";
        
        var createdCategory = await _connection.QuerySingleAsync<Category>(sql, category);
        return createdCategory;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        var sql = @"UPDATE Categories 
                    SET Name = @Name, Description = @Description 
                    WHERE Id = @Id 
                    RETURNING *";
        
        var updatedCategory = await _connection.QuerySingleAsync<Category>(sql, category);
        return updatedCategory;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Categories WHERE Id = @Id";
        var result = await _connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }
}