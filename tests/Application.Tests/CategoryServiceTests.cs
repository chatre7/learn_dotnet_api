using Application.DTOs;
using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly Mock<ILogger<CategoryService>> _mockLogger;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockLogger = new Mock<ILogger<CategoryService>>();
        _mockCacheService = new Mock<ICacheService>();
        _categoryService = new CategoryService(_mockCategoryRepository.Object, _mockLogger.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ReturnsAllCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Technology", Description = "Tech related posts" },
            new Category { Id = 2, Name = "Science", Description = "Science related posts" }
        };

        var categoryDtos = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        }).ToList();

        _mockCacheService.Setup(cache => cache.GetOrCreateAsync<IEnumerable<CategoryDto>>(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<CategoryDto>>>>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(categoryDtos);

        // Act
        var result = await _categoryService.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetCategoryByIdAsync_ExistingCategory_ReturnsCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Technology", Description = "Tech related posts" };
        var categoryDto = new CategoryDto { Id = 1, Name = "Technology", Description = "Tech related posts" };

        _mockCacheService.Setup(cache => cache.GetOrCreateAsync<CategoryDto?>(It.IsAny<string>(), It.IsAny<Func<Task<CategoryDto?>>>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(categoryDto);

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Technology", result.Name);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_NonExistingCategory_ReturnsNull()
    {
        // Arrange
        _mockCacheService.Setup(cache => cache.GetOrCreateAsync<CategoryDto?>(It.IsAny<string>(), It.IsAny<Func<Task<CategoryDto?>>>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync((CategoryDto?)null);

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateCategoryAsync_ValidCategory_CreatesAndReturnsCategory()
    {
        // Arrange
        var createCategoryDto = new CreateCategoryDto { Name = "Technology", Description = "Tech related posts" };
        var category = new Category { Id = 1, Name = "Technology", Description = "Tech related posts" };
        
        _mockCategoryRepository.Setup(repo => repo.CreateAsync(It.IsAny<Category>())).ReturnsAsync(category);

        // Act
        var result = await _categoryService.CreateCategoryAsync(createCategoryDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Technology", result.Name);
        _mockCategoryRepository.Verify(repo => repo.CreateAsync(It.IsAny<Category>()), Times.Once);
        _mockCacheService.Verify(cache => cache.Remove("all_categories"), Times.Once);
    }

    [Fact]
    public async Task UpdateCategoryAsync_ExistingCategory_UpdatesAndReturnsCategory()
    {
        // Arrange
        var updateCategoryDto = new UpdateCategoryDto { Name = "Updated Tech", Description = "Updated tech posts" };
        var existingCategory = new Category { Id = 1, Name = "Technology", Description = "Tech related posts" };
        var updatedCategory = new Category { Id = 1, Name = "Updated Tech", Description = "Updated tech posts" };
        
        _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingCategory);
        _mockCategoryRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(updatedCategory);

        // Act
        var result = await _categoryService.UpdateCategoryAsync(1, updateCategoryDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Tech", result.Name);
        _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Category>()), Times.Once);
        _mockCacheService.Verify(cache => cache.Remove("category_1"), Times.Once);
        _mockCacheService.Verify(cache => cache.Remove("all_categories"), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoryAsync_ExistingCategory_ReturnsTrue()
    {
        // Arrange
        _mockCategoryRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _categoryService.DeleteCategoryAsync(1);

        // Assert
        Assert.True(result);
        _mockCategoryRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        _mockCacheService.Verify(cache => cache.Remove("category_1"), Times.Once);
        _mockCacheService.Verify(cache => cache.Remove("all_categories"), Times.Once);
    }
}