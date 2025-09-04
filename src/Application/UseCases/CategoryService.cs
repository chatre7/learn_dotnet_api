using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.UseCases;

/// <summary>
/// Service for managing categories in the application.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryService"/> class.
    /// </summary>
    /// <param name="categoryRepository">The category repository.</param>
    /// <param name="logger">The logger instance.</param>
    public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    /// <summary>
    /// Gets all categories asynchronously.
    /// </summary>
    /// <returns>A collection of category DTOs.</returns>
    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        _logger.LogInformation("Getting all categories");
        var categories = await _categoryRepository.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} categories", categories.Count());
        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        });
    }

    /// <summary>
    /// Gets a category by its ID asynchronously.
    /// </summary>
    /// <param name="id">The category ID.</param>
    /// <returns>The category DTO if found; otherwise, null.</returns>
    /// <exception cref="ValidationException">Thrown when the ID is less than or equal to zero.</exception>
    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        _logger.LogInformation("Getting category by ID: {Id}", id);
        
        if (id <= 0)
        {
            _logger.LogWarning("Invalid category ID: {Id}", id);
            throw new ValidationException("Category ID must be greater than zero");
        }

        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            _logger.LogWarning("Category not found with ID: {Id}", id);
        }
        else
        {
            _logger.LogInformation("Category found with ID: {Id}", id);
        }
        
        return category; // Will be null if not found, which is handled by the API controller
    }

    /// <summary>
    /// Creates a new category asynchronously.
    /// </summary>
    /// <param name="createCategoryDto">The DTO containing category creation data.</param>
    /// <returns>The created category DTO.</returns>
    /// <exception cref="ValidationException">Thrown when the category name is null or empty.</exception>
    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
    {
        _logger.LogInformation("Creating new category with name: {Name}", createCategoryDto.Name);
        
        if (string.IsNullOrWhiteSpace(createCategoryDto.Name))
        {
            _logger.LogWarning("Category name is required but was null or empty");
            throw new ValidationException("Category name is required");
        }

        var category = new Domain.Entities.Category
        {
            Name = createCategoryDto.Name ?? string.Empty,
            Description = createCategoryDto.Description ?? string.Empty
        };

        var createdCategory = await _categoryRepository.CreateAsync(category);
        _logger.LogInformation("Category created successfully with ID: {Id}", createdCategory.Id);
        
        return new CategoryDto
        {
            Id = createdCategory.Id,
            Name = createdCategory.Name,
            Description = createdCategory.Description
        };
    }

    /// <summary>
    /// Updates an existing category asynchronously.
    /// </summary>
    /// <param name="id">The ID of the category to update.</param>
    /// <param name="updateCategoryDto">The DTO containing updated category data.</param>
    /// <returns>The updated category DTO.</returns>
    /// <exception cref="ValidationException">Thrown when the ID is less than or equal to zero or the category name is null or empty.</exception>
    /// <exception cref="EntityNotFoundException">Thrown when no category with the specified ID is found.</exception>
    public async Task<CategoryDto> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
    {
        _logger.LogInformation("Updating category with ID: {Id}", id);
        
        if (id <= 0)
        {
            _logger.LogWarning("Invalid category ID: {Id}", id);
            throw new ValidationException("Category ID must be greater than zero");
        }

        if (string.IsNullOrWhiteSpace(updateCategoryDto.Name))
        {
            _logger.LogWarning("Category name is required but was null or empty for ID: {Id}", id);
            throw new ValidationException("Category name is required");
        }

        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
        {
            _logger.LogWarning("Category not found with ID: {Id}", id);
            throw new EntityNotFoundException($"Category with ID {id} not found");
        }

        existingCategory.Name = updateCategoryDto.Name ?? existingCategory.Name;
        existingCategory.Description = updateCategoryDto.Description ?? existingCategory.Description;

        var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);
        _logger.LogInformation("Category updated successfully with ID: {Id}", updatedCategory.Id);

        return new CategoryDto
        {
            Id = updatedCategory.Id,
            Name = updatedCategory.Name,
            Description = updatedCategory.Description
        };
    }

    /// <summary>
    /// Deletes a category by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <returns>True if the category was deleted; otherwise, false.</returns>
    /// <exception cref="ValidationException">Thrown when the ID is less than or equal to zero.</exception>
    public async Task<bool> DeleteCategoryAsync(int id)
    {
        _logger.LogInformation("Deleting category with ID: {Id}", id);
        
        if (id <= 0)
        {
            _logger.LogWarning("Invalid category ID: {Id}", id);
            throw new ValidationException("Category ID must be greater than zero");
        }

        var result = await _categoryRepository.DeleteAsync(id);
        if (result)
        {
            _logger.LogInformation("Category deleted successfully with ID: {Id}", id);
        }
        else
        {
            _logger.LogWarning("Category not found for deletion with ID: {Id}", id);
        }
        
        return result;
    }
}