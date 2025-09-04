using Application.DTOs;
using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _userService = new UserService(_mockUserRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Name = "John Doe", Email = "john@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetUserByIdAsync_ExistingUser_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = 1, Name = "John Doe", Email = "john@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John Doe", result.Name);
        _mockUserRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetUserByIdAsync_NonExistingUser_ReturnsNull()
    {
        // Arrange
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserByIdAsync(1);

        // Assert
        Assert.Null(result);
        _mockUserRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_ValidUser_CreatesAndReturnsUser()
    {
        // Arrange
        var createUserDto = new CreateUserDto { Name = "John Doe", Email = "john@example.com" };
        var user = new User { Id = 1, Name = "John Doe", Email = "john@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        
        _mockUserRepository.Setup(repo => repo.CreateAsync(It.IsAny<User>())).ReturnsAsync(user);

        // Act
        var result = await _userService.CreateUserAsync(createUserDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John Doe", result.Name);
        _mockUserRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ExistingUser_UpdatesAndReturnsUser()
    {
        // Arrange
        var updateUserDto = new UpdateUserDto { Name = "John Smith", Email = "johnsmith@example.com" };
        var existingUser = new User { Id = 1, Name = "John Doe", Email = "john@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var updatedUser = new User { Id = 1, Name = "John Smith", Email = "johnsmith@example.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingUser);
        _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>())).ReturnsAsync(updatedUser);

        // Act
        var result = await _userService.UpdateUserAsync(1, updateUserDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John Smith", result.Name);
        _mockUserRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        _mockUserRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _userService.DeleteUserAsync(1);

        // Assert
        Assert.True(result);
        _mockUserRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }
}