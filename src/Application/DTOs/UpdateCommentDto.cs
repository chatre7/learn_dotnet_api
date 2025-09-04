namespace Application.DTOs;

/// <summary>
/// Data transfer object for updating an existing comment.
/// </summary>
public class UpdateCommentDto
{
    /// <summary>
    /// Gets or sets the content of the comment.
    /// </summary>
    public string? Content { get; set; }
}