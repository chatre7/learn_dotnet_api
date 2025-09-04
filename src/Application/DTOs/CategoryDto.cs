namespace Application.DTOs;

/// <summary>
/// Data transfer object for category information.
/// </summary>
public class CategoryDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the category.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}