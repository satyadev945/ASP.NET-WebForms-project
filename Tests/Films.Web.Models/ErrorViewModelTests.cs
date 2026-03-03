using Xunit;
using Films.Web.Models;

namespace Films.Web.Models.Tests;

/// <summary>
/// Unit tests for ErrorViewModel
/// </summary>
public class ErrorViewModelTests
{
    [Fact]
    public void RequestId_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var model = new ErrorViewModel();
        var expectedRequestId = "test-request-id-123";

        // Act
        model.RequestId = expectedRequestId;

        // Assert
        Assert.Equal(expectedRequestId, model.RequestId);
    }

    [Fact]
    public void RequestId_SetToNull_ReturnsNull()
    {
        // Arrange
        var model = new ErrorViewModel();

        // Act
        model.RequestId = null;

        // Assert
        Assert.Null(model.RequestId);
    }

    [Fact]
    public void ShowRequestId_WhenRequestIdIsNull_ReturnsFalse()
    {
        // Arrange
        var model = new ErrorViewModel
        {
            RequestId = null
        };

        // Act
        var result = model.ShowRequestId;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShowRequestId_WhenRequestIdIsEmpty_ReturnsFalse()
    {
        // Arrange
        var model = new ErrorViewModel
        {
            RequestId = string.Empty
        };

        // Act
        var result = model.ShowRequestId;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShowRequestId_WhenRequestIdIsWhitespace_ReturnsFalse()
    {
        // Arrange
        var model = new ErrorViewModel
        {
            RequestId = "   "
        };

        // Act
        var result = model.ShowRequestId;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShowRequestId_WhenRequestIdHasValue_ReturnsTrue()
    {
        // Arrange
        var model = new ErrorViewModel
        {
            RequestId = "valid-request-id"
        };

        // Act
        var result = model.ShowRequestId;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ErrorViewModel_DefaultConstructor_InitializesWithNullRequestId()
    {
        // Act
        var model = new ErrorViewModel();

        // Assert
        Assert.Null(model.RequestId);
        Assert.False(model.ShowRequestId);
    }

    [Theory]
    [InlineData("request-1")]
    [InlineData("abc123")]
    [InlineData("0123456789")]
    [InlineData("special-chars-!@#")]
    public void ShowRequestId_WithVariousValidRequestIds_ReturnsTrue(string requestId)
    {
        // Arrange
        var model = new ErrorViewModel
        {
            RequestId = requestId
        };

        // Act
        var result = model.ShowRequestId;

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void ShowRequestId_WithInvalidRequestIds_ReturnsFalse(string? requestId)
    {
        // Arrange
        var model = new ErrorViewModel
        {
            RequestId = requestId
        };

        // Act
        var result = model.ShowRequestId;

        // Assert
        Assert.False(result);
    }
}
