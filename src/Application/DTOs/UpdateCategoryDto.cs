namespace Application.DTOs;

/// <summary>
/// Data transfer object for updating an existing category.
/// </summary>
public class UpdateCategoryDto
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