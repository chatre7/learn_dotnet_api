using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

/// <summary>
/// Data transfer object for creating a new post.
/// </summary>
public class CreatePostDto
{
    /// <summary>
    /// Gets or sets the title of the post.
    /// </summary>
    [Required(ErrorMessage = "Post title is required")]
    [StringLength(200, ErrorMessage = "Post title cannot exceed 200 characters")]
    public string? Title { get; set; }
    
    /// <summary>
    /// Gets or sets the content of the post.
    /// </summary>
    [Required(ErrorMessage = "Post content is required")]
    [StringLength(5000, ErrorMessage = "Post content cannot exceed 5000 characters")]
    public string? Content { get; set; }
    
    /// <summary>
    /// Gets or sets the user ID associated with the post.
    /// </summary>
    [Required(ErrorMessage = "User ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "User ID must be greater than 0")]
    public int UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the category ID associated with the post.
    /// </summary>
    [Required(ErrorMessage = "Category ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Category ID must be greater than 0")]
    public int CategoryId { get; set; }
}