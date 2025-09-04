namespace Domain.Entities;

/// <summary>
/// Represents a post entity in the domain.
/// </summary>
public class Post
{
    /// <summary>
    /// Gets or sets the unique identifier for the post.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the title of the post.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the content of the post.
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user ID associated with the post.
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the category ID associated with the post.
    /// </summary>
    public int CategoryId { get; set; }
    
    /// <summary>
    /// Gets or sets the creation date and time of the post.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the last update date and time of the post.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}