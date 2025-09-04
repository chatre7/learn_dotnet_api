using Application.DTOs;

namespace Application.Interfaces;

/// <summary>
/// Interface for category service operations.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Gets all categories asynchronously.
    /// </summary>
    /// <returns>A collection of category DTOs.</returns>
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    
    /// <summary>
    /// Gets a category by its ID asynchronously.
    /// </summary>
    /// <param name="id">The category ID.</param>
    /// <returns>The category DTO if found; otherwise, null.</returns>
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    
    /// <summary>
    /// Creates a new category asynchronously.
    /// </summary>
    /// <param name="createCategoryDto">The DTO containing category creation data.</param>
    /// <returns>The created category DTO.</returns>
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
    
    /// <summary>
    /// Updates an existing category asynchronously.
    /// </summary>
    /// <param name="id">The ID of the category to update.</param>
    /// <param name="updateCategoryDto">The DTO containing updated category data.</param>
    /// <returns>The updated category DTO.</returns>
    Task<CategoryDto> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto);
    
    /// <summary>
    /// Deletes a category by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <returns>True if the category was deleted; otherwise, false.</returns>
    Task<bool> DeleteCategoryAsync(int id);
}