using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

/// <summary>
/// Data transfer object for creating a new category.
/// </summary>
public class CreateCategoryDto
{
    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    [StringLength(500, ErrorMessage = "Category description cannot exceed 500 characters")]
    public string? Description { get; set; }
}