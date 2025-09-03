using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.UseCases;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

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

    public async Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto)
    {
        var comment = new Domain.Entities.Comment
        {
            Content = createCommentDto.Content,
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

    public async Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDto updateCommentDto)
    {
        var existingComment = await _commentRepository.GetByIdAsync(id);
        if (existingComment == null)
        {
            throw new Exception($"Comment with ID {id} not found");
        }

        existingComment.Content = updateCommentDto.Content;

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

    public async Task<bool> DeleteCommentAsync(int id)
    {
        return await _commentRepository.DeleteAsync(id);
    }
}