using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.UseCases;

/// <summary>
/// Service for managing comments in the application.
/// </summary>
public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentService"/> class.
    /// </summary>
    /// <param name="commentRepository">The comment repository.</param>
    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    /// <summary>
    /// Gets all comments asynchronously.
    /// </summary>
    /// <returns>A collection of comment DTOs.</returns>
    public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync()
    {
        var comments = await _commentRepository.GetAllAsync();
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
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null) return null;

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
        var comment = new Domain.Entities.Comment
        {
            Content = createCommentDto.Content ?? string.Empty,
            UserId = createCommentDto.UserId,
            PostId = createCommentDto.PostId
        };

        var createdComment = await _commentRepository.CreateAsync(comment);

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
        var existingComment = await _commentRepository.GetByIdAsync(id);
        if (existingComment == null)
        {
            throw new Exception($"Comment with ID {id} not found");
        }

        existingComment.Content = updateCommentDto.Content ?? existingComment.Content;

        var updatedComment = await _commentRepository.UpdateAsync(existingComment);

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
        return await _commentRepository.DeleteAsync(id);
    }
}