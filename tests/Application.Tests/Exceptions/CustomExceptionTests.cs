using Domain.Exceptions;

namespace Application.Tests.Exceptions;

/// <summary>
/// Unit tests for custom exceptions.
/// </summary>
public class CustomExceptionTests
{
    /// <summary>
    /// Tests that EntityNotFoundException can be instantiated with no parameters.
    /// </summary>
    [Fact]
    public void EntityNotFoundException_CanBeInstantiatedWithNoParameters()
    {
        // Act
        var exception = new EntityNotFoundException();

        // Assert
        Assert.NotNull(exception);
    }

    /// <summary>
    /// Tests that EntityNotFoundException can be instantiated with a message.
    /// </summary>
    [Fact]
    public void EntityNotFoundException_CanBeInstantiatedWithMessage()
    {
        // Arrange
        var message = "Entity not found";

        // Act
        var exception = new EntityNotFoundException(message);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(message, exception.Message);
    }

    /// <summary>
    /// Tests that EntityNotFoundException can be instantiated with a message and inner exception.
    /// </summary>
    [Fact]
    public void EntityNotFoundException_CanBeInstantiatedWithMessageAndInnerException()
    {
        // Arrange
        var message = "Entity not found";
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new EntityNotFoundException(message, innerException);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }

    /// <summary>
    /// Tests that ValidationException can be instantiated with no parameters.
    /// </summary>
    [Fact]
    public void ValidationException_CanBeInstantiatedWithNoParameters()
    {
        // Act
        var exception = new ValidationException();

        // Assert
        Assert.NotNull(exception);
    }

    /// <summary>
    /// Tests that ValidationException can be instantiated with a message.
    /// </summary>
    [Fact]
    public void ValidationException_CanBeInstantiatedWithMessage()
    {
        // Arrange
        var message = "Validation failed";

        // Act
        var exception = new ValidationException(message);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(message, exception.Message);
    }

    /// <summary>
    /// Tests that ValidationException can be instantiated with a message and inner exception.
    /// </summary>
    [Fact]
    public void ValidationException_CanBeInstantiatedWithMessageAndInnerException()
    {
        // Arrange
        var message = "Validation failed";
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new ValidationException(message, innerException);

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}