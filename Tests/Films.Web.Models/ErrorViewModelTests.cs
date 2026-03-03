using Xunit;
using Films.Web.Models;

namespace Films.Web.Tests.Models;

/// <summary>
/// Unit tests for ErrorViewModel class
/// </summary>
public class ErrorViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var viewModel = new ErrorViewModel();

        // Assert
        Assert.Null(viewModel.RequestId);
        Assert.False(viewModel.ShowRequestId);
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
    public void ShowRequestId_ShouldReturnTrue_WhenRequestIdIsNotEmpty()
    {
        // Arrange
        var viewModel = new ErrorViewModel
        {
            RequestId = "test-request-id"
        };

        // Act
        var result = viewModel.ShowRequestId;

        // Assert
        Assert.True(result);
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

    [Theory]
    [InlineData("request-123")]
    [InlineData("abc-def-ghi")]
    [InlineData("12345")]
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
    public void RequestId_ShouldAllowNullValue()
    {
        // Arrange
        var viewModel = new ErrorViewModel
        {
            RequestId = "test"
        };

        // Act
        viewModel.RequestId = null;

        // Assert
        Assert.Null(viewModel.RequestId);
        Assert.False(viewModel.ShowRequestId);
    }

    [Fact]
    public void RequestId_ShouldUpdateShowRequestId_WhenChanged()
    {
        // Arrange
        var viewModel = new ErrorViewModel
        {
            RequestId = null
        };
        Assert.False(viewModel.ShowRequestId);

        // Act
        viewModel.RequestId = "new-request-id";

        // Assert
        Assert.True(viewModel.ShowRequestId);
    }
}
