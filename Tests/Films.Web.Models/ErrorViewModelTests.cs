using Xunit;
using Films.Web.Models;

namespace Films.Web.Tests.Models;

/// <summary>
/// Unit tests for ErrorViewModel class
/// </summary>
public class ErrorViewModelTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance()
    {
        // Arrange & Act
        var viewModel = new ErrorViewModel();

        // Assert
        Assert.NotNull(viewModel);
    }

    [Fact]
    public void RequestId_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var viewModel = new ErrorViewModel();

        // Assert
        Assert.Null(viewModel.RequestId);
    }

    [Fact]
    public void RequestId_ShouldSetAndGetValue()
    {
        // Arrange
        var viewModel = new ErrorViewModel();
        var expectedRequestId = "test-request-id-123";

        // Act
        viewModel.RequestId = expectedRequestId;

        // Assert
        Assert.Equal(expectedRequestId, viewModel.RequestId);
    }

    [Fact]
    public void ShowRequestId_ShouldReturnFalse_WhenRequestIdIsNull()
    {
        // Arrange
        var viewModel = new ErrorViewModel
        {
            RequestId = null
        };

        // Act
        var result = viewModel.ShowRequestId;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShowRequestId_ShouldReturnFalse_WhenRequestIdIsEmpty()
    {
        // Arrange
        var viewModel = new ErrorViewModel
        {
            RequestId = string.Empty
        };

        // Act
        var result = viewModel.ShowRequestId;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShowRequestId_ShouldReturnFalse_WhenRequestIdIsWhitespace()
    {
        // Arrange
        var viewModel = new ErrorViewModel
        {
            RequestId = "   "
        };

        // Act
        var result = viewModel.ShowRequestId;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShowRequestId_ShouldReturnTrue_WhenRequestIdHasValue()
    {
        // Arrange
        var viewModel = new ErrorViewModel
        {
            RequestId = "valid-request-id"
        };

        // Act
        var result = viewModel.ShowRequestId;

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("request-123")]
    [InlineData("abc-def-ghi")]
    [InlineData("12345")]
    [InlineData("test")]
    public void ShowRequestId_ShouldReturnTrue_ForVariousValidRequestIds(string requestId)
    {
        // Arrange
        var viewModel = new ErrorViewModel
        {
            RequestId = requestId
        };

        // Act
        var result = viewModel.ShowRequestId;

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void ShowRequestId_ShouldReturnFalse_ForInvalidRequestIds(string? requestId)
    {
        // Arrange
        var viewModel = new ErrorViewModel
        {
            RequestId = requestId
        };

        // Act
        var result = viewModel.ShowRequestId;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void RequestId_ShouldAllowMultipleAssignments()
    {
        // Arrange
        var viewModel = new ErrorViewModel();

        // Act & Assert
        viewModel.RequestId = "first-id";
        Assert.Equal("first-id", viewModel.RequestId);
        Assert.True(viewModel.ShowRequestId);

        viewModel.RequestId = "second-id";
        Assert.Equal("second-id", viewModel.RequestId);
        Assert.True(viewModel.ShowRequestId);

        viewModel.RequestId = null;
        Assert.Null(viewModel.RequestId);
        Assert.False(viewModel.ShowRequestId);
    }
}
