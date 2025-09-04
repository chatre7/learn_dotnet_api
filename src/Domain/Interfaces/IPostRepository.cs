using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Interface for post repository operations.
/// </summary>
public interface IPostRepository
{
    /// <summary>
    /// Gets a post by its ID asynchronously.
    /// </summary>
    /// <param name="id">The post ID.</param>
    /// <returns>The post entity if found; otherwise, null.</returns>
    Task<Post?> GetByIdAsync(int id);
    
    /// <summary>
    /// Gets all posts asynchronously.
    /// </summary>
    /// <returns>A collection of post entities.</returns>
    Task<IEnumerable<Post>> GetAllAsync();
    
    /// <summary>
    /// Creates a new post asynchronously.
    /// </summary>
    /// <param name="post">The post entity to create.</param>
    /// <returns>The created post entity.</returns>
    Task<Post> CreateAsync(Post post);
    
    /// <summary>
    /// Updates an existing post asynchronously.
    /// </summary>
    /// <param name="post">The post entity to update.</param>
    /// <returns>The updated post entity.</returns>
    Task<Post> UpdateAsync(Post post);
    
    /// <summary>
    /// Deletes a post by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the post to delete.</param>
    /// <returns>True if the post was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id);
}