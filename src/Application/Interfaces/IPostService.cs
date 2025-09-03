using Application.DTOs;

namespace Application.Interfaces;

public interface IPostService
{
    Task<IEnumerable<PostDto>> GetAllPostsAsync();
    Task<PostDto?> GetPostByIdAsync(int id);
    Task<PostDto> CreatePostAsync(CreatePostDto createPostDto);
    Task<PostDto> UpdatePostAsync(int id, UpdatePostDto updatePostDto);
    Task<bool> DeletePostAsync(int id);
}