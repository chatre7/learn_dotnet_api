namespace Domain.Entities;

/// <summary>
/// Represents a comment entity in the domain.
/// </summary>
public class Comment
{
    /// <summary>
    /// Gets or sets the unique identifier for the comment.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the content of the comment.
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user ID associated with the comment.
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the post ID associated with the comment.
    /// </summary>
    public int PostId { get; set; }
    
    /// <summary>
    /// Gets or sets the creation date and time of the comment.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}