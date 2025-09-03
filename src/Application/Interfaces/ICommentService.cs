using Application.DTOs;

namespace Application.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetAllCommentsAsync();
    Task<CommentDto?> GetCommentByIdAsync(int id);
    Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto);
    Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDto updateCommentDto);
    Task<bool> DeleteCommentAsync(int id);
}