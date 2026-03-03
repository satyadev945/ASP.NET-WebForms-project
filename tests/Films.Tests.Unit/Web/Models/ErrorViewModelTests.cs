using Xunit;
using Films.Web.Models;

namespace Films.Tests.Unit.Web.Models;

public class ErrorViewModelTests
{
    [Fact]
    public void RequestId_SetAndGet_ReturnsCorrectValue()
    {
        var model = new ErrorViewModel();
        var expectedRequestId = "test-request-id-123";
        model.RequestId = expectedRequestId;
        Assert.Equal(expectedRequestId, model.RequestId);
    }

    [Fact]
    public void ShowRequestId_WhenRequestIdIsNull_ReturnsFalse()
    {
        var model = new ErrorViewModel { RequestId = null };
        Assert.False(model.ShowRequestId);
    }

    [Fact]
    public void ShowRequestId_WhenRequestIdIsEmpty_ReturnsFalse()
    {
        var model = new ErrorViewModel { RequestId = string.Empty };
        Assert.False(model.ShowRequestId);
    }

    [Fact]
    public void ShowRequestId_WhenRequestIdHasValue_ReturnsTrue()
    {
        var model = new ErrorViewModel { RequestId = "valid-request-id" };
        Assert.True(model.ShowRequestId);
    }

    [Theory]
    [InlineData("request-1")]
    [InlineData("abc123")]
    [InlineData("special-chars-!@#")]
    public void ShowRequestId_WithVariousValidRequestIds_ReturnsTrue(string requestId)
    {
        var model = new ErrorViewModel { RequestId = requestId };
        Assert.True(model.ShowRequestId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShowRequestId_WithInvalidRequestIds_ReturnsFalse(string? requestId)
    {
        var model = new ErrorViewModel { RequestId = requestId };
        Assert.False(model.ShowRequestId);
    }
}
