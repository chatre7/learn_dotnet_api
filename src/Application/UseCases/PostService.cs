using Application.DTOs;
using Application.Interfaces;
using Application.Utilities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases;

/// <summary>
/// Service for managing posts in the application.
/// </summary>
public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<PostService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostService"/> class.
    /// </summary>
    /// <param name="postRepository">The post repository.</param>
    /// <param name="logger">The logger instance.</param>
    public PostService(IPostRepository postRepository, ILogger<PostService> logger)
    {
        _postRepository = postRepository;
        _logger = logger;
    }

    /// <summary>
    /// Gets all posts asynchronously.
    /// </summary>
    /// <returns>A collection of post DTOs.</returns>
    public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
    {
        _logger.LogInformation("Getting all posts");
        var posts = await _postRepository.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} posts", posts.Count());
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

    /// <summary>
    /// Gets a post by its ID asynchronously.
    /// </summary>
    /// <param name="id">The post ID.</param>
    /// <returns>The post DTO if found; otherwise, null.</returns>
    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        _logger.LogInformation("Getting post by ID: {Id}", id);
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
        {
            _logger.LogWarning("Post not found with ID: {Id}", id);
            return null;
        }
        
        _logger.LogInformation("Post found with ID: {Id}", id);
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

    /// <summary>
    /// Creates a new post asynchronously.
    /// </summary>
    /// <param name="createPostDto">The DTO containing post creation data.</param>
    /// <returns>The created post DTO.</returns>
    public async Task<PostDto> CreatePostAsync(CreatePostDto createPostDto)
    {
        _logger.LogInformation("Creating new post with title: {Title}", createPostDto.Title);
        
        // Sanitize inputs
        var sanitizedTitle = InputSanitizer.SanitizeString(createPostDto.Title);
        var sanitizedContent = InputSanitizer.SanitizeString(createPostDto.Content);
        
        var post = new Domain.Entities.Post
        {
            Title = sanitizedTitle ?? string.Empty,
            Content = sanitizedContent ?? string.Empty,
            UserId = createPostDto.UserId,
            CategoryId = createPostDto.CategoryId
        };

        var createdPost = await _postRepository.CreateAsync(post);
        _logger.LogInformation("Post created successfully with ID: {Id}", createdPost.Id);

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

    /// <summary>
    /// Updates an existing post asynchronously.
    /// </summary>
    /// <param name="id">The ID of the post to update.</param>
    /// <param name="updatePostDto">The DTO containing updated post data.</param>
    /// <returns>The updated post DTO.</returns>
    /// <exception cref="Exception">Thrown when no post with the specified ID is found.</exception>
    public async Task<PostDto> UpdatePostAsync(int id, UpdatePostDto updatePostDto)
    {
        _logger.LogInformation("Updating post with ID: {Id}", id);
        
        // Sanitize inputs
        var sanitizedTitle = InputSanitizer.SanitizeString(updatePostDto.Title);
        var sanitizedContent = InputSanitizer.SanitizeString(updatePostDto.Content);
        
        var existingPost = await _postRepository.GetByIdAsync(id);
        if (existingPost == null)
        {
            _logger.LogWarning("Post not found with ID: {Id}", id);
            throw new Exception($"Post with ID {id} not found");
        }

        existingPost.Title = sanitizedTitle ?? existingPost.Title;
        existingPost.Content = sanitizedContent ?? existingPost.Content;
        existingPost.CategoryId = updatePostDto.CategoryId;

        var updatedPost = await _postRepository.UpdateAsync(existingPost);
        _logger.LogInformation("Post updated successfully with ID: {Id}", updatedPost.Id);

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

    /// <summary>
    /// Deletes a post by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the post to delete.</param>
    /// <returns>True if the post was deleted; otherwise, false.</returns>
    public async Task<bool> DeletePostAsync(int id)
    {
        _logger.LogInformation("Deleting post with ID: {Id}", id);
        var result = await _postRepository.DeleteAsync(id);
        if (result)
        {
            _logger.LogInformation("Post deleted successfully with ID: {Id}", id);
        }
        else
        {
            _logger.LogWarning("Post not found for deletion with ID: {Id}", id);
        }
        return result;
    }
}