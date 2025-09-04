using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Interface for category repository operations.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Gets a category by its ID asynchronously.
    /// </summary>
    /// <param name="id">The category ID.</param>
    /// <returns>The category entity if found; otherwise, null.</returns>
    Task<Category?> GetByIdAsync(int id);
    
    /// <summary>
    /// Gets all categories asynchronously.
    /// </summary>
    /// <returns>A collection of category entities.</returns>
    Task<IEnumerable<Category>> GetAllAsync();
    
    /// <summary>
    /// Creates a new category asynchronously.
    /// </summary>
    /// <param name="category">The category entity to create.</param>
    /// <returns>The created category entity.</returns>
    Task<Category> CreateAsync(Category category);
    
    /// <summary>
    /// Updates an existing category asynchronously.
    /// </summary>
    /// <param name="category">The category entity to update.</param>
    /// <returns>The updated category entity.</returns>
    Task<Category> UpdateAsync(Category category);
    
    /// <summary>
    /// Deletes a category by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <returns>True if the category was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id);
}