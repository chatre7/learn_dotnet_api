using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Interface for user service operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets all users asynchronously.
    /// </summary>
    /// <returns>A collection of user DTOs.</returns>
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    
    /// <summary>
    /// Gets a user by its ID asynchronously.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The user DTO if found; otherwise, null.</returns>
    Task<UserDto?> GetUserByIdAsync(int id);
    
    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="createUserDto">The DTO containing user creation data.</param>
    /// <returns>The created user DTO.</returns>
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    
    /// <summary>
    /// Updates an existing user asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="updateUserDto">The DTO containing updated user data.</param>
    /// <returns>The updated user DTO.</returns>
    Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    
    /// <summary>
    /// Deletes a user by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>True if the user was deleted; otherwise, false.</returns>
    Task<bool> DeleteUserAsync(int id);
}