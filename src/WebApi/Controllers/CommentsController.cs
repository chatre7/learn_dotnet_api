using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers;

/// <summary>
/// Controller for managing comments.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly ILogger<CommentsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentsController"/> class.
    /// </summary>
    /// <param name="commentService">The comment service.</param>
    /// <param name="logger">The logger instance.</param>
    public CommentsController(ICommentService commentService, ILogger<CommentsController> logger)
    {
        _commentService = commentService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all comments.
    /// </summary>
    /// <returns>A collection of comment DTOs.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments()
    {
        _logger.LogInformation("Getting all comments");
        var comments = await _commentService.GetAllCommentsAsync();
        _logger.LogInformation("Retrieved {Count} comments", comments.Count());
        return Ok(comments);
    }

    /// <summary>
    /// Gets a comment by its ID.
    /// </summary>
    /// <param name="id">The comment ID.</param>
    /// <returns>The comment DTO if found; otherwise, NotFound.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(int id)
    {
        _logger.LogInformation("Getting comment by ID: {Id}", id);
        var comment = await _commentService.GetCommentByIdAsync(id);
        if (comment == null)
        {
            _logger.LogWarning("Comment not found with ID: {Id}", id);
            return NotFound();
        }
        _logger.LogInformation("Comment found with ID: {Id}", id);
        return Ok(comment);
    }

    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="createCommentDto">The DTO containing comment creation data.</param>
    /// <returns>The created comment DTO.</returns>
    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment(CreateCommentDto createCommentDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for comment creation");
            return BadRequest(ModelState);
        }
        
        _logger.LogInformation("Creating new comment for post ID: {PostId}", createCommentDto.PostId);
        var comment = await _commentService.CreateCommentAsync(createCommentDto);
        _logger.LogInformation("Comment created successfully with ID: {Id}", comment.Id);
        return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
    }

    /// <summary>
    /// Updates an existing comment.
    /// </summary>
    /// <param name="id">The ID of the comment to update.</param>
    /// <param name="updateCommentDto">The DTO containing updated comment data.</param>
    /// <returns>The updated comment DTO.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, UpdateCommentDto updateCommentDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for comment update with ID: {Id}", id);
            return BadRequest(ModelState);
        }
        
        try
        {
            _logger.LogInformation("Updating comment with ID: {Id}", id);
            var comment = await _commentService.UpdateCommentAsync(id, updateCommentDto);
            _logger.LogInformation("Comment updated successfully with ID: {Id}", comment.Id);
            return Ok(comment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating comment with ID: {Id}", id);
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a comment by its ID.
    /// </summary>
    /// <param name="id">The ID of the comment to delete.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        _logger.LogInformation("Deleting comment with ID: {Id}", id);
        var result = await _commentService.DeleteCommentAsync(id);
        if (!result)
        {
            _logger.LogWarning("Comment not found for deletion with ID: {Id}", id);
            return NotFound();
        }
        _logger.LogInformation("Comment deleted successfully with ID: {Id}", id);
        return NoContent();
    }
}