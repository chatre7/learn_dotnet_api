namespace Domain.Entities;

/// <summary>
/// Represents a category entity in the domain.
/// </summary>
public class Category
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