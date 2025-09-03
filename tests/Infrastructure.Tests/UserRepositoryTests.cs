using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Npgsql;
using Testcontainers.PostgreSql;

namespace Infrastructure.Tests;

public class UserRepositoryTests : IAsyncLifetime
{
    private PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:16")
        .WithDatabase("testdb")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private NpgsqlConnection _connection = null!;
    private UserRepository _userRepository = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        _connection = new NpgsqlConnection(_container.GetConnectionString());
        await _connection.OpenAsync();
        
        // Initialize database schema
        await DatabaseInitializer.InitializeDatabaseAsync(_container.GetConnectionString());
        
        _userRepository = new UserRepository(_connection);
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
        await _container.DisposeAsync();
    }

    [Fact]
    public async Task CreateAsync_ValidUser_ReturnsCreatedUser()
    {
        // Arrange
        var user = new User
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };

        // Act
        var result = await _userRepository.CreateAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(0, result.Id);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("john.doe@example.com", result.Email);
        Assert.NotEqual(default(DateTime), result.CreatedAt);
        Assert.NotEqual(default(DateTime), result.UpdatedAt);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingUser_ReturnsUser()
    {
        // Arrange
        var user = new User
        {
            Name = "Jane Smith",
            Email = "jane.smith@example.com"
        };

        var createdUser = await _userRepository.CreateAsync(user);

        // Act
        var result = await _userRepository.GetByIdAsync(createdUser.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdUser.Id, result.Id);
        Assert.Equal("Jane Smith", result.Name);
        Assert.Equal("jane.smith@example.com", result.Email);
    }

    [Fact]
    public async Task GetAllAsync_MultipleUsers_ReturnsAllUsers()
    {
        // Arrange
        var user1 = new User { Name = "User 1", Email = "user1@example.com" };
        var user2 = new User { Name = "User 2", Email = "user2@example.com" };

        await _userRepository.CreateAsync(user1);
        await _userRepository.CreateAsync(user2);

        // Act
        var result = await _userRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() >= 2);
    }

    [Fact]
    public async Task UpdateAsync_ExistingUser_ReturnsUpdatedUser()
    {
        // Arrange
        var user = new User
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };

        var createdUser = await _userRepository.CreateAsync(user);
        createdUser.Name = "John Smith";
        createdUser.Email = "john.smith@example.com";

        // Act
        var result = await _userRepository.UpdateAsync(createdUser);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdUser.Id, result.Id);
        Assert.Equal("John Smith", result.Name);
        Assert.Equal("john.smith@example.com", result.Email);
    }

    [Fact]
    public async Task DeleteAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var user = new User
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };

        var createdUser = await _userRepository.CreateAsync(user);

        // Act
        var result = await _userRepository.DeleteAsync(createdUser.Id);

        // Assert
        Assert.True(result);
    }
}