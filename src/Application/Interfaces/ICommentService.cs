using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Interface for comment service operations.
/// </summary>
public interface ICommentService
{
    /// <summary>
    /// Gets all comments asynchronously.
    /// </summary>
    /// <returns>A collection of comment DTOs.</returns>
    Task<IEnumerable<CommentDto>> GetAllCommentsAsync();
    
    /// <summary>
    /// Gets a comment by its ID asynchronously.
    /// </summary>
    /// <param name="id">The comment ID.</param>
    /// <returns>The comment DTO if found; otherwise, null.</returns>
    Task<CommentDto?> GetCommentByIdAsync(int id);
    
    /// <summary>
    /// Creates a new comment asynchronously.
    /// </summary>
    /// <param name="createCommentDto">The DTO containing comment creation data.</param>
    /// <returns>The created comment DTO.</returns>
    Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto);
    
    /// <summary>
    /// Updates an existing comment asynchronously.
    /// </summary>
    /// <param name="id">The ID of the comment to update.</param>
    /// <param name="updateCommentDto">The DTO containing updated comment data.</param>
    /// <returns>The updated comment DTO.</returns>
    Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDto updateCommentDto);
    
    /// <summary>
    /// Deletes a comment by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the comment to delete.</param>
    /// <returns>True if the comment was deleted; otherwise, false.</returns>
    Task<bool> DeleteCommentAsync(int id);
}