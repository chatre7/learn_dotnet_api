using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Interface for user repository operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by its ID asynchronously.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    Task<User?> GetByIdAsync(int id);
    
    /// <summary>
    /// Gets all users asynchronously.
    /// </summary>
    /// <returns>A collection of user entities.</returns>
    Task<IEnumerable<User>> GetAllAsync();
    
    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="user">The user entity to create.</param>
    /// <returns>The created user entity.</returns>
    Task<User> CreateAsync(User user);
    
    /// <summary>
    /// Updates an existing user asynchronously.
    /// </summary>
    /// <param name="user">The user entity to update.</param>
    /// <returns>The updated user entity.</returns>
    Task<User> UpdateAsync(User user);
    
    /// <summary>
    /// Deletes a user by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>True if the user was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id);
}