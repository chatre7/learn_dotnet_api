namespace Application.DTOs;

/// <summary>
/// Data transfer object for creating a new comment.
/// </summary>
public class CreateCommentDto
{
    /// <summary>
    /// Gets or sets the content of the comment.
    /// </summary>
    public string? Content { get; set; }
    
    /// <summary>
    /// Gets or sets the user ID associated with the comment.
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the post ID associated with the comment.
    /// </summary>
    public int PostId { get; set; }
}