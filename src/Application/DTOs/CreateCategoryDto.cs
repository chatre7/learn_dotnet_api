namespace Application.DTOs;

/// <summary>
/// Data transfer object for creating a new category.
/// </summary>
public class CreateCategoryDto
{
    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    public string? Description { get; set; }
}