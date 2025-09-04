using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Interface for comment repository operations.
/// </summary>
public interface ICommentRepository
{
    /// <summary>
    /// Gets a comment by its ID asynchronously.
    /// </summary>
    /// <param name="id">The comment ID.</param>
    /// <returns>The comment entity if found; otherwise, null.</returns>
    Task<Comment?> GetByIdAsync(int id);
    
    /// <summary>
    /// Gets all comments asynchronously.
    /// </summary>
    /// <returns>A collection of comment entities.</returns>
    Task<IEnumerable<Comment>> GetAllAsync();
    
    /// <summary>
    /// Creates a new comment asynchronously.
    /// </summary>
    /// <param name="comment">The comment entity to create.</param>
    /// <returns>The created comment entity.</returns>
    Task<Comment> CreateAsync(Comment comment);
    
    /// <summary>
    /// Updates an existing comment asynchronously.
    /// </summary>
    /// <param name="comment">The comment entity to update.</param>
    /// <returns>The updated comment entity.</returns>
    Task<Comment> UpdateAsync(Comment comment);
    
    /// <summary>
    /// Deletes a comment by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the comment to delete.</param>
    /// <returns>True if the comment was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id);
}