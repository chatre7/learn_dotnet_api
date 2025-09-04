using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.UseCases;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
    {
        var posts = await _postRepository.GetAllAsync();
        return posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Content = p.Content,
            UserId = p.UserId,
            CategoryId = p.CategoryId,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });
    }

    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null) return null;

        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            UserId = post.UserId,
            CategoryId = post.CategoryId,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt
        };
    }

    public async Task<PostDto> CreatePostAsync(CreatePostDto createPostDto)
    {
        var post = new Domain.Entities.Post
        {
            Title = createPostDto.Title ?? string.Empty,
            Content = createPostDto.Content ?? string.Empty,
            UserId = createPostDto.UserId,
            CategoryId = createPostDto.CategoryId
        };

        var createdPost = await _postRepository.CreateAsync(post);

        return new PostDto
        {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Content = createdPost.Content,
            UserId = createdPost.UserId,
            CategoryId = createdPost.CategoryId,
            CreatedAt = createdPost.CreatedAt,
            UpdatedAt = createdPost.UpdatedAt
        };
    }

    public async Task<PostDto> UpdatePostAsync(int id, UpdatePostDto updatePostDto)
    {
        var existingPost = await _postRepository.GetByIdAsync(id);
        if (existingPost == null)
        {
            throw new Exception($"Post with ID {id} not found");
        }

        existingPost.Title = updatePostDto.Title ?? existingPost.Title;
        existingPost.Content = updatePostDto.Content ?? existingPost.Content;
        existingPost.CategoryId = updatePostDto.CategoryId;

        var updatedPost = await _postRepository.UpdateAsync(existingPost);

        return new PostDto
        {
            Id = updatedPost.Id,
            Title = updatedPost.Title,
            Content = updatedPost.Content,
            UserId = updatedPost.UserId,
            CategoryId = updatedPost.CategoryId,
            CreatedAt = updatedPost.CreatedAt,
            UpdatedAt = updatedPost.UpdatedAt
        };
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        return await _postRepository.DeleteAsync(id);
    }
}