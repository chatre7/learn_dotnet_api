using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.UseCases;

/// <summary>
/// Service for managing users in the application.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Gets all users asynchronously.
    /// </summary>
    /// <returns>A collection of user DTOs.</returns>
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
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
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

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
        var user = new Domain.Entities.User
        {
            Name = createUserDto.Name ?? string.Empty,
            Email = createUserDto.Email ?? string.Empty
        };

        var createdUser = await _userRepository.CreateAsync(user);

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
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            throw new Exception($"User with ID {id} not found");
        }

        existingUser.Name = updateUserDto.Name ?? existingUser.Name;
        existingUser.Email = updateUserDto.Email ?? existingUser.Email;

        var updatedUser = await _userRepository.UpdateAsync(existingUser);

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
        return await _userRepository.DeleteAsync(id);
    }
}