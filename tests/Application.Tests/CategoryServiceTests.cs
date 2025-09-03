using Application.DTOs;
using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.Tests;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _categoryService = new CategoryService(_mockCategoryRepository.Object);
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

        _mockCategoryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

        // Act
        var result = await _categoryService.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockCategoryRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_ExistingCategory_ReturnsCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Technology", Description = "Tech related posts" };
        _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(category);

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Technology", result.Name);
        _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_NonExistingCategory_ReturnsNull()
    {
        // Arrange
        _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Category)null);

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(1);

        // Assert
        Assert.Null(result);
        _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
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
    }
}