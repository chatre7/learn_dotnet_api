using Application.DTOs;
using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.Tests;

public class CommentServiceTests
{
    private readonly Mock<ICommentRepository> _mockCommentRepository;
    private readonly CommentService _commentService;

    public CommentServiceTests()
    {
        _mockCommentRepository = new Mock<ICommentRepository>();
        _commentService = new CommentService(_mockCommentRepository.Object);
    }

    [Fact]
    public async Task GetAllCommentsAsync_ReturnsAllComments()
    {
        // Arrange
        var comments = new List<Comment>
        {
            new Comment { Id = 1, Content = "Great post!", UserId = 1, PostId = 1, CreatedAt = DateTime.UtcNow },
            new Comment { Id = 2, Content = "Thanks for sharing", UserId = 2, PostId = 1, CreatedAt = DateTime.UtcNow }
        };

        _mockCommentRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(comments);

        // Act
        var result = await _commentService.GetAllCommentsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockCommentRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetCommentByIdAsync_ExistingComment_ReturnsComment()
    {
        // Arrange
        var comment = new Comment { Id = 1, Content = "Great post!", UserId = 1, PostId = 1, CreatedAt = DateTime.UtcNow };
        _mockCommentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(comment);

        // Act
        var result = await _commentService.GetCommentByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Great post!", result.Content);
        _mockCommentRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetCommentByIdAsync_NonExistingComment_ReturnsNull()
    {
        // Arrange
        _mockCommentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Comment?)null);

        // Act
        var result = await _commentService.GetCommentByIdAsync(1);

        // Assert
        Assert.Null(result);
        _mockCommentRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreateCommentAsync_ValidComment_CreatesAndReturnsComment()
    {
        // Arrange
        var createCommentDto = new CreateCommentDto { Content = "Great post!", UserId = 1, PostId = 1 };
        var comment = new Comment { Id = 1, Content = "Great post!", UserId = 1, PostId = 1, CreatedAt = DateTime.UtcNow };
        
        _mockCommentRepository.Setup(repo => repo.CreateAsync(It.IsAny<Comment>())).ReturnsAsync(comment);

        // Act
        var result = await _commentService.CreateCommentAsync(createCommentDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Great post!", result.Content);
        _mockCommentRepository.Verify(repo => repo.CreateAsync(It.IsAny<Comment>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCommentAsync_ExistingComment_UpdatesAndReturnsComment()
    {
        // Arrange
        var updateCommentDto = new UpdateCommentDto { Content = "Updated comment" };
        var existingComment = new Comment { Id = 1, Content = "Great post!", UserId = 1, PostId = 1, CreatedAt = DateTime.UtcNow };
        var updatedComment = new Comment { Id = 1, Content = "Updated comment", UserId = 1, PostId = 1, CreatedAt = DateTime.UtcNow };
        
        _mockCommentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingComment);
        _mockCommentRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Comment>())).ReturnsAsync(updatedComment);

        // Act
        var result = await _commentService.UpdateCommentAsync(1, updateCommentDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated comment", result.Content);
        _mockCommentRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mockCommentRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Comment>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCommentAsync_ExistingComment_ReturnsTrue()
    {
        // Arrange
        _mockCommentRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _commentService.DeleteCommentAsync(1);

        // Assert
        Assert.True(result);
        _mockCommentRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }
}