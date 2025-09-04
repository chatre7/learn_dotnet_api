using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Exceptions;

namespace Application.UseCases;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        });
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        if (id <= 0)
            throw new ValidationException("Category ID must be greater than zero");

        var category = await _categoryRepository.GetByIdAsync(id);
        return category; // Will be null if not found, which is handled by the API controller
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
    {
        if (string.IsNullOrWhiteSpace(createCategoryDto.Name))
            throw new ValidationException("Category name is required");

        var category = new Domain.Entities.Category
        {
            Name = createCategoryDto.Name ?? string.Empty,
            Description = createCategoryDto.Description ?? string.Empty
        };

        var createdCategory = await _categoryRepository.CreateAsync(category);
        return new CategoryDto
        {
            Id = createdCategory.Id,
            Name = createdCategory.Name,
            Description = createdCategory.Description
        };
    }

    public async Task<CategoryDto> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
    {
        if (id <= 0)
            throw new ValidationException("Category ID must be greater than zero");

        if (string.IsNullOrWhiteSpace(updateCategoryDto.Name))
            throw new ValidationException("Category name is required");

        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
        {
            throw new EntityNotFoundException($"Category with ID {id} not found");
        }

        existingCategory.Name = updateCategoryDto.Name ?? existingCategory.Name;
        existingCategory.Description = updateCategoryDto.Description ?? existingCategory.Description;

        var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);

        return new CategoryDto
        {
            Id = updatedCategory.Id,
            Name = updatedCategory.Name,
            Description = updatedCategory.Description
        };
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        if (id <= 0)
            throw new ValidationException("Category ID must be greater than zero");

        return await _categoryRepository.DeleteAsync(id);
    }
}