namespace Application.DTOs;

/// <summary>
/// Data transfer object for creating a new post.
/// </summary>
public class CreatePostDto
{
    /// <summary>
    /// Gets or sets the title of the post.
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// Gets or sets the content of the post.
    /// </summary>
    public string? Content { get; set; }
    
    /// <summary>
    /// Gets or sets the user ID associated with the post.
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the category ID associated with the post.
    /// </summary>
    public int CategoryId { get; set; }
}