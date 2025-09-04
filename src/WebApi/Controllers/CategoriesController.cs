using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers;

/// <summary>
/// Controller for managing categories.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoriesController"/> class.
    /// </summary>
    /// <param name="categoryService">The category service.</param>
    /// <param name="logger">The logger instance.</param>
    public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns>A collection of category DTOs.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        _logger.LogInformation("Getting all categories");
        var categories = await _categoryService.GetAllCategoriesAsync();
        _logger.LogInformation("Retrieved {Count} categories", categories.Count());
        return Ok(categories);
    }

    /// <summary>
    /// Gets a category by its ID.
    /// </summary>
    /// <param name="id">The category ID.</param>
    /// <returns>The category DTO if found; otherwise, NotFound.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        _logger.LogInformation("Getting category by ID: {Id}", id);
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            _logger.LogWarning("Category not found with ID: {Id}", id);
            return NotFound();
        }
        _logger.LogInformation("Category found with ID: {Id}", id);
        return Ok(category);
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="createCategoryDto">The DTO containing category creation data.</param>
    /// <returns>The created category DTO.</returns>
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        _logger.LogInformation("Creating new category with name: {Name}", createCategoryDto.Name);
        var category = await _categoryService.CreateCategoryAsync(createCategoryDto);
        _logger.LogInformation("Category created successfully with ID: {Id}", category.Id);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="id">The ID of the category to update.</param>
    /// <param name="updateCategoryDto">The DTO containing updated category data.</param>
    /// <returns>The updated category DTO.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto updateCategoryDto)
    {
        try
        {
            _logger.LogInformation("Updating category with ID: {Id}", id);
            var category = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
            _logger.LogInformation("Category updated successfully with ID: {Id}", category.Id);
            return Ok(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category with ID: {Id}", id);
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <returns>NoContent if successful; otherwise, NotFound.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        _logger.LogInformation("Deleting category with ID: {Id}", id);
        var result = await _categoryService.DeleteCategoryAsync(id);
        if (!result)
        {
            _logger.LogWarning("Category not found for deletion with ID: {Id}", id);
            return NotFound();
        }
        _logger.LogInformation("Category deleted successfully with ID: {Id}", id);
        return NoContent();
    }
}