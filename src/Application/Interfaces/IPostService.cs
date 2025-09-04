using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Interface for post service operations.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Gets all posts asynchronously.
    /// </summary>
    /// <returns>A collection of post DTOs.</returns>
    Task<IEnumerable<PostDto>> GetAllPostsAsync();
    
    /// <summary>
    /// Gets a post by its ID asynchronously.
    /// </summary>
    /// <param name="id">The post ID.</param>
    /// <returns>The post DTO if found; otherwise, null.</returns>
    Task<PostDto?> GetPostByIdAsync(int id);
    
    /// <summary>
    /// Creates a new post asynchronously.
    /// </summary>
    /// <param name="createPostDto">The DTO containing post creation data.</param>
    /// <returns>The created post DTO.</returns>
    Task<PostDto> CreatePostAsync(CreatePostDto createPostDto);
    
    /// <summary>
    /// Updates an existing post asynchronously.
    /// </summary>
    /// <param name="id">The ID of the post to update.</param>
    /// <param name="updatePostDto">The DTO containing updated post data.</param>
    /// <returns>The updated post DTO.</returns>
    Task<PostDto> UpdatePostAsync(int id, UpdatePostDto updatePostDto);
    
    /// <summary>
    /// Deletes a post by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the post to delete.</param>
    /// <returns>True if the post was deleted; otherwise, false.</returns>
    Task<bool> DeletePostAsync(int id);
}