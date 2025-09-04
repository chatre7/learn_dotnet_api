using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers;

/// <summary>
/// Controller for managing posts.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ILogger<PostsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostsController"/> class.
    /// </summary>
    /// <param name="postService">The post service.</param>
    /// <param name="logger">The logger instance.</param>
    public PostsController(IPostService postService, ILogger<PostsController> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all posts.
    /// </summary>
    /// <returns>A collection of post DTOs.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
    {
        _logger.LogInformation("Getting all posts");
        var posts = await _postService.GetAllPostsAsync();
        _logger.LogInformation("Retrieved {Count} posts", posts.Count());
        return Ok(posts);
    }

    /// <summary>
    /// Gets a post by its ID.
    /// </summary>
    /// <param name="id">The post ID.</param>
    /// <returns>The post DTO if found; otherwise, NotFound.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(int id)
    {
        _logger.LogInformation("Getting post by ID: {Id}", id);
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
        {
            _logger.LogWarning("Post not found with ID: {Id}", id);
            return NotFound();
        }
        _logger.LogInformation("Post found with ID: {Id}", id);
        return Ok(post);
    }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="createPostDto">The DTO containing post creation data.</param>
    /// <returns>The created post DTO.</returns>
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost(CreatePostDto createPostDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for post creation");
            return BadRequest(ModelState);
        }
        
        _logger.LogInformation("Creating new post with title: {Title}", createPostDto.Title);
        var post = await _postService.CreatePostAsync(createPostDto);
        _logger.LogInformation("Post created successfully with ID: {Id}", post.Id);
        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
    }

    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="id">The ID of the post to update.</param>
    /// <param name="updatePostDto">The DTO containing updated post data.</param>
    /// <returns>The updated post DTO.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, UpdatePostDto updatePostDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for post update with ID: {Id}", id);
            return BadRequest(ModelState);
        }
        
        try
        {
            _logger.LogInformation("Updating post with ID: {Id}", id);
            var post = await _postService.UpdatePostAsync(id, updatePostDto);
            _logger.LogInformation("Post updated successfully with ID: {Id}", post.Id);
            return Ok(post);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating post with ID: {Id}", id);
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a post by its ID.
    /// </summary>
    /// <param name="id">The ID of the post to delete.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        _logger.LogInformation("Deleting post with ID: {Id}", id);
        var result = await _postService.DeletePostAsync(id);
        if (!result)
        {
            _logger.LogWarning("Post not found for deletion with ID: {Id}", id);
            return NotFound();
        }
        _logger.LogInformation("Post deleted successfully with ID: {Id}", id);
        return NoContent();
    }
}