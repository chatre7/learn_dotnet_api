using Application.DTOs;
using Application.Interfaces;
using Application.Utilities;
using Domain.Interfaces;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCases;

/// <summary>
/// Service for managing categories in the application.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryService> _logger;
    private readonly ICacheService _cacheService;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryService"/> class.
    /// </summary>
    /// <param name="categoryRepository">The category repository.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="cacheService">The cache service instance.</param>
    public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger, ICacheService cacheService)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
        _cacheService = cacheService;
    }

    /// <summary>
    /// Gets all categories asynchronously.
    /// </summary>
    /// <returns>A collection of category DTOs.</returns>
    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        _logger.LogInformation("Getting all categories");
        
        var cacheKey = "all_categories";
        var categories = await _cacheService.GetOrCreateAsync<IEnumerable<CategoryDto>>(
            cacheKey,
            async () =>
            {
                _logger.LogInformation("Cache miss for all categories, fetching from database");
                var dbCategories = await _categoryRepository.GetAllAsync();
                _logger.LogInformation("Retrieved {Count} categories from database", dbCategories.Count());
                return dbCategories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToList();
            },
            _cacheExpiration);

        _logger.LogInformation("Returning {Count} categories", categories.Count());
        return categories;
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

        var cacheKey = $"category_{id}";
        var category = await _cacheService.GetOrCreateAsync<CategoryDto?>(
            cacheKey,
            async () =>
            {
                _logger.LogInformation("Cache miss for category {Id}, fetching from database", id);
                var dbCategory = await _categoryRepository.GetByIdAsync(id);
                if (dbCategory == null)
                {
                    _logger.LogWarning("Category not found with ID: {Id}", id);
                    return null;
                }
                
                _logger.LogInformation("Category found with ID: {Id}", id);
                return new CategoryDto
                {
                    Id = dbCategory.Id,
                    Name = dbCategory.Name,
                    Description = dbCategory.Description
                };
            },
            _cacheExpiration);

        return category;
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
        
        // Sanitize inputs
        var sanitizedName = InputSanitizer.SanitizeString(createCategoryDto.Name);
        var sanitizedDescription = InputSanitizer.SanitizeString(createCategoryDto.Description);
        
        if (string.IsNullOrWhiteSpace(sanitizedName))
        {
            _logger.LogWarning("Category name is required but was null or empty after sanitization");
            throw new ValidationException("Category name is required");
        }

        var category = new Domain.Entities.Category
        {
            Name = sanitizedName,
            Description = sanitizedDescription
        };

        var createdCategory = await _categoryRepository.CreateAsync(category);
        _logger.LogInformation("Category created successfully with ID: {Id}", createdCategory.Id);
        
        // Invalidate cache for all categories since we added a new one
        _cacheService.Remove("all_categories");
        
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
        
        // Sanitize inputs
        var sanitizedName = InputSanitizer.SanitizeString(updateCategoryDto.Name);
        var sanitizedDescription = InputSanitizer.SanitizeString(updateCategoryDto.Description);
        
        if (id <= 0)
        {
            _logger.LogWarning("Invalid category ID: {Id}", id);
            throw new ValidationException("Category ID must be greater than zero");
        }

        if (string.IsNullOrWhiteSpace(sanitizedName))
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

        existingCategory.Name = sanitizedName;
        existingCategory.Description = sanitizedDescription;

        var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);
        _logger.LogInformation("Category updated successfully with ID: {Id}", updatedCategory.Id);

        // Invalidate cache for this specific category and all categories
        _cacheService.Remove($"category_{id}");
        _cacheService.Remove("all_categories");

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
            // Invalidate cache for this specific category and all categories
            _cacheService.Remove($"category_{id}");
            _cacheService.Remove("all_categories");
        }
        else
        {
            _logger.LogWarning("Category not found for deletion with ID: {Id}", id);
        }
        
        return result;
    }
}