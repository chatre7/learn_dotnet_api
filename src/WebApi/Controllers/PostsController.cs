using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
/// Controller for managing posts.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostsController"/> class.
    /// </summary>
    /// <param name="postService">The post service.</param>
    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    /// <summary>
    /// Gets all posts.
    /// </summary>
    /// <returns>A collection of post DTOs.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
    {
        var posts = await _postService.GetAllPostsAsync();
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
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }
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
        var post = await _postService.CreatePostAsync(createPostDto);
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
        try
        {
            var post = await _postService.UpdatePostAsync(id, updatePostDto);
            return Ok(post);
        }
        catch (Exception ex)
        {
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
        var result = await _postService.DeletePostAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}