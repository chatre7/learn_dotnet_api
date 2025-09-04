using Application.DTOs;
using Application.Interfaces;
using Application.Utilities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases;

/// <summary>
/// Service for managing comments in the application.
/// </summary>
public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<CommentService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentService"/> class.
    /// </summary>
    /// <param name="commentRepository">The comment repository.</param>
    /// <param name="logger">The logger instance.</param>
    public CommentService(ICommentRepository commentRepository, ILogger<CommentService> logger)
    {
        _commentRepository = commentRepository;
        _logger = logger;
    }

    /// <summary>
    /// Gets all comments asynchronously.
    /// </summary>
    /// <returns>A collection of comment DTOs.</returns>
    public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync()
    {
        _logger.LogInformation("Getting all comments");
        var comments = await _commentRepository.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} comments", comments.Count());
        return comments.Select(c => new CommentDto
        {
            Id = c.Id,
            Content = c.Content,
            UserId = c.UserId,
            PostId = c.PostId,
            CreatedAt = c.CreatedAt
        });
    }

    /// <summary>
    /// Gets a comment by its ID asynchronously.
    /// </summary>
    /// <param name="id">The comment ID.</param>
    /// <returns>The comment DTO if found; otherwise, null.</returns>
    public async Task<CommentDto?> GetCommentByIdAsync(int id)
    {
        _logger.LogInformation("Getting comment by ID: {Id}", id);
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
        {
            _logger.LogWarning("Comment not found with ID: {Id}", id);
            return null;
        }
        
        _logger.LogInformation("Comment found with ID: {Id}", id);
        return new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            UserId = comment.UserId,
            PostId = comment.PostId,
            CreatedAt = comment.CreatedAt
        };
    }

    /// <summary>
    /// Creates a new comment asynchronously.
    /// </summary>
    /// <param name="createCommentDto">The DTO containing comment creation data.</param>
    /// <returns>The created comment DTO.</returns>
    public async Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto)
    {
        _logger.LogInformation("Creating new comment for post ID: {PostId}", createCommentDto.PostId);
        
        // Sanitize inputs
        var sanitizedContent = InputSanitizer.SanitizeString(createCommentDto.Content);
        
        var comment = new Domain.Entities.Comment
        {
            Content = sanitizedContent ?? string.Empty,
            UserId = createCommentDto.UserId,
            PostId = createCommentDto.PostId
        };

        var createdComment = await _commentRepository.CreateAsync(comment);
        _logger.LogInformation("Comment created successfully with ID: {Id}", createdComment.Id);

        return new CommentDto
        {
            Id = createdComment.Id,
            Content = createdComment.Content,
            UserId = createdComment.UserId,
            PostId = createdComment.PostId,
            CreatedAt = createdComment.CreatedAt
        };
    }

    /// <summary>
    /// Updates an existing comment asynchronously.
    /// </summary>
    /// <param name="id">The ID of the comment to update.</param>
    /// <param name="updateCommentDto">The DTO containing updated comment data.</param>
    /// <returns>The updated comment DTO.</returns>
    /// <exception cref="Exception">Thrown when no comment with the specified ID is found.</exception>
    public async Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDto updateCommentDto)
    {
        _logger.LogInformation("Updating comment with ID: {Id}", id);
        
        // Sanitize inputs
        var sanitizedContent = InputSanitizer.SanitizeString(updateCommentDto.Content);
        
        var existingComment = await _commentRepository.GetByIdAsync(id);
        if (existingComment == null)
        {
            _logger.LogWarning("Comment not found with ID: {Id}", id);
            throw new Exception($"Comment with ID {id} not found");
        }

        existingComment.Content = sanitizedContent ?? existingComment.Content;

        var updatedComment = await _commentRepository.UpdateAsync(existingComment);
        _logger.LogInformation("Comment updated successfully with ID: {Id}", updatedComment.Id);

        return new CommentDto
        {
            Id = updatedComment.Id,
            Content = updatedComment.Content,
            UserId = updatedComment.UserId,
            PostId = updatedComment.PostId,
            CreatedAt = updatedComment.CreatedAt
        };
    }

    /// <summary>
    /// Deletes a comment by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the comment to delete.</param>
    /// <returns>True if the comment was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteCommentAsync(int id)
    {
        _logger.LogInformation("Deleting comment with ID: {Id}", id);
        var result = await _commentRepository.DeleteAsync(id);
        if (result)
        {
            _logger.LogInformation("Comment deleted successfully with ID: {Id}", id);
        }
        else
        {
            _logger.LogWarning("Comment not found for deletion with ID: {Id}", id);
        }
        return result;
    }
}