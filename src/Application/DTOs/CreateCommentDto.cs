using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

/// <summary>
/// Data transfer object for creating a new comment.
/// </summary>
public class CreateCommentDto
{
    /// <summary>
    /// Gets or sets the content of the comment.
    /// </summary>
    [Required(ErrorMessage = "Comment content is required")]
    [StringLength(1000, ErrorMessage = "Comment content cannot exceed 1000 characters")]
    public string? Content { get; set; }
    
    /// <summary>
    /// Gets or sets the user ID associated with the comment.
    /// </summary>
    [Required(ErrorMessage = "User ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "User ID must be greater than 0")]
    public int UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the post ID associated with the comment.
    /// </summary>
    [Required(ErrorMessage = "Post ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Post ID must be greater than 0")]
    public int PostId { get; set; }
}