using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

/// <summary>
/// Data transfer object for updating an existing comment.
/// </summary>
public class UpdateCommentDto
{
    /// <summary>
    /// Gets or sets the content of the comment.
    /// </summary>
    [Required(ErrorMessage = "Comment content is required")]
    [StringLength(1000, ErrorMessage = "Comment content cannot exceed 1000 characters")]
    public string? Content { get; set; }
}