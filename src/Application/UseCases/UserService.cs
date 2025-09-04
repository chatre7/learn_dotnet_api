using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases;

/// <summary>
/// Service for managing users in the application.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="logger">The logger instance.</param>
    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <summary>
    /// Gets all users asynchronously.
    /// </summary>
    /// <returns>A collection of user DTOs.</returns>
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        _logger.LogInformation("Getting all users");
        var users = await _userRepository.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} users", users.Count());
        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
            UpdatedAt = u.UpdatedAt
        });
    }

    /// <summary>
    /// Gets a user by its ID asynchronously.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The user DTO if found; otherwise, null.</returns>
    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        _logger.LogInformation("Getting user by ID: {Id}", id);
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {Id}", id);
            return null;
        }
        
        _logger.LogInformation("User found with ID: {Id}", id);
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="createUserDto">The DTO containing user creation data.</param>
    /// <returns>The created user DTO.</returns>
    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        _logger.LogInformation("Creating new user with name: {Name}", createUserDto.Name);
        
        var user = new Domain.Entities.User
        {
            Name = createUserDto.Name ?? string.Empty,
            Email = createUserDto.Email ?? string.Empty
        };

        var createdUser = await _userRepository.CreateAsync(user);
        _logger.LogInformation("User created successfully with ID: {Id}", createdUser.Id);

        return new UserDto
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Email = createdUser.Email,
            CreatedAt = createdUser.CreatedAt,
            UpdatedAt = createdUser.UpdatedAt
        };
    }

    /// <summary>
    /// Updates an existing user asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="updateUserDto">The DTO containing updated user data.</param>
    /// <returns>The updated user DTO.</returns>
    /// <exception cref="Exception">Thrown when no user with the specified ID is found.</exception>
    public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        _logger.LogInformation("Updating user with ID: {Id}", id);
        
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            _logger.LogWarning("User not found with ID: {Id}", id);
            throw new Exception($"User with ID {id} not found");
        }

        existingUser.Name = updateUserDto.Name ?? existingUser.Name;
        existingUser.Email = updateUserDto.Email ?? existingUser.Email;

        var updatedUser = await _userRepository.UpdateAsync(existingUser);
        _logger.LogInformation("User updated successfully with ID: {Id}", updatedUser.Id);

        return new UserDto
        {
            Id = updatedUser.Id,
            Name = updatedUser.Name,
            Email = updatedUser.Email,
            CreatedAt = updatedUser.CreatedAt,
            UpdatedAt = updatedUser.UpdatedAt
        };
    }

    /// <summary>
    /// Deletes a user by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>True if the user was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteUserAsync(int id)
    {
        _logger.LogInformation("Deleting user with ID: {Id}", id);
        var result = await _userRepository.DeleteAsync(id);
        if (result)
        {
            _logger.LogInformation("User deleted successfully with ID: {Id}", id);
        }
        else
        {
            _logger.LogWarning("User not found for deletion with ID: {Id}", id);
        }
        return result;
    }
}