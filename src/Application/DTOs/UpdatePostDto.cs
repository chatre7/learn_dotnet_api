namespace Application.DTOs;

/// <summary>
/// Data transfer object for updating an existing post.
/// </summary>
public class UpdatePostDto
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
    /// Gets or sets the category ID associated with the post.
    /// </summary>
    public int CategoryId { get; set; }
}