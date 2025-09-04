using Application.DTOs;
using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests;

public class PostServiceTests
{
    private readonly Mock<IPostRepository> _mockPostRepository;
    private readonly Mock<ILogger<PostService>> _mockLogger;
    private readonly PostService _postService;

    public PostServiceTests()
    {
        _mockPostRepository = new Mock<IPostRepository>();
        _mockLogger = new Mock<ILogger<PostService>>();
        _postService = new PostService(_mockPostRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllPostsAsync_ReturnsAllPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            new Post { Id = 1, Title = "First Post", Content = "Content of first post", UserId = 1, CategoryId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Post { Id = 2, Title = "Second Post", Content = "Content of second post", UserId = 2, CategoryId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        _mockPostRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(posts);

        // Act
        var result = await _postService.GetAllPostsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockPostRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetPostByIdAsync_ExistingPost_ReturnsPost()
    {
        // Arrange
        var post = new Post { Id = 1, Title = "First Post", Content = "Content of first post", UserId = 1, CategoryId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        _mockPostRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(post);

        // Act
        var result = await _postService.GetPostByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("First Post", result.Title);
        _mockPostRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetPostByIdAsync_NonExistingPost_ReturnsNull()
    {
        // Arrange
        _mockPostRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Post?)null);

        // Act
        var result = await _postService.GetPostByIdAsync(1);

        // Assert
        Assert.Null(result);
        _mockPostRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreatePostAsync_ValidPost_CreatesAndReturnsPost()
    {
        // Arrange
        var createPostDto = new CreatePostDto { Title = "New Post", Content = "Content of new post", UserId = 1, CategoryId = 1 };
        var post = new Post { Id = 1, Title = "New Post", Content = "Content of new post", UserId = 1, CategoryId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        
        _mockPostRepository.Setup(repo => repo.CreateAsync(It.IsAny<Post>())).ReturnsAsync(post);

        // Act
        var result = await _postService.CreatePostAsync(createPostDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Post", result.Title);
        _mockPostRepository.Verify(repo => repo.CreateAsync(It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePostAsync_ExistingPost_UpdatesAndReturnsPost()
    {
        // Arrange
        var updatePostDto = new UpdatePostDto { Title = "Updated Post", Content = "Updated content", CategoryId = 2 };
        var existingPost = new Post { Id = 1, Title = "First Post", Content = "Content of first post", UserId = 1, CategoryId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var updatedPost = new Post { Id = 1, Title = "Updated Post", Content = "Updated content", UserId = 1, CategoryId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        
        _mockPostRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingPost);
        _mockPostRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Post>())).ReturnsAsync(updatedPost);

        // Act
        var result = await _postService.UpdatePostAsync(1, updatePostDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Post", result.Title);
        _mockPostRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mockPostRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public async Task DeletePostAsync_ExistingPost_ReturnsTrue()
    {
        // Arrange
        _mockPostRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _postService.DeletePostAsync(1);

        // Assert
        Assert.True(result);
        _mockPostRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }
}