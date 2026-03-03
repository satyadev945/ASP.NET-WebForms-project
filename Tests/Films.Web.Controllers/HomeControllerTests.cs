using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Films.Web.Controllers;
using Films.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Films.Web.Controllers.Tests;

/// <summary>
/// Unit tests for HomeController
/// </summary>
public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _mockLogger;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _mockLogger = new Mock<ILogger<HomeController>>();
        _controller = new HomeController(_mockLogger.Object);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidLogger_CreatesInstance()
    {
        // Act
        var controller = new HomeController(_mockLogger.Object);

        // Assert
        Assert.NotNull(controller);
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new HomeController(null!));
    }

    #endregion

    #region Index Tests

    [Fact]
    public void Index_ReturnsViewResult()
    {
        // Act
        var result = _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Index_LogsInformation()
    {
        // Act
        _controller.Index();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Home page accessed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region About Tests

    [Fact]
    public void About_ReturnsViewResult()
    {
        // Act
        var result = _controller.About();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void About_LogsInformation()
    {
        // Act
        _controller.About();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("About page accessed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Contact Tests

    [Fact]
    public void Contact_ReturnsViewResult()
    {
        // Act
        var result = _controller.Contact();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Contact_LogsInformation()
    {
        // Act
        _controller.Contact();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Contact page accessed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion

    #region Privacy Tests

    [Fact]
    public void Privacy_ReturnsViewResult()
    {
        // Act
        var result = _controller.Privacy();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    #endregion

    #region Error Tests

    [Fact]
    public void Error_ReturnsViewResult()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = "test-trace-id";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = _controller.Error();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error_ReturnsViewWithErrorViewModel()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = "test-trace-id";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = _controller.Error();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.NotNull(model);
    }

    [Fact]
    public void Error_SetsRequestIdFromTraceIdentifier()
    {
        // Arrange
        var expectedTraceId = "test-trace-id-123";
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = expectedTraceId;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = _controller.Error();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.Equal(expectedTraceId, model.RequestId);
    }

    [Fact]
    public void Error_UsesActivityIdWhenAvailable()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = _controller.Error();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.NotNull(model.RequestId);
        
        activity.Stop();
    }

    [Fact]
    public void Error_HasResponseCacheAttribute()
    {
        // Arrange
        var methodInfo = typeof(HomeController).GetMethod("Error");

        // Act
        var attributes = methodInfo?.GetCustomAttributes(typeof(ResponseCacheAttribute), false);

        // Assert
        Assert.NotNull(attributes);
        Assert.Single(attributes);
        var cacheAttribute = attributes[0] as ResponseCacheAttribute;
        Assert.NotNull(cacheAttribute);
        Assert.Equal(0, cacheAttribute.Duration);
        Assert.Equal(ResponseCacheLocation.None, cacheAttribute.Location);
        Assert.True(cacheAttribute.NoStore);
    }

    #endregion

    #region Multiple Action Tests

    [Theory]
    [InlineData("Index")]
    [InlineData("About")]
    [InlineData("Contact")]
    [InlineData("Privacy")]
    public void Actions_ReturnViewResult(string actionName)
    {
        // Arrange
        var methodInfo = typeof(HomeController).GetMethod(actionName);

        // Act
        var result = methodInfo?.Invoke(_controller, null);

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Index_CanBeCalledMultipleTimes()
    {
        // Act
        var result1 = _controller.Index();
        var result2 = _controller.Index();
        var result3 = _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result1);
        Assert.IsType<ViewResult>(result2);
        Assert.IsType<ViewResult>(result3);
    }

    [Fact]
    public void AllActions_DoNotReturnNull()
    {
        // Act & Assert
        Assert.NotNull(_controller.Index());
        Assert.NotNull(_controller.About());
        Assert.NotNull(_controller.Contact());
        Assert.NotNull(_controller.Privacy());
        
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        Assert.NotNull(_controller.Error());
    }

    #endregion

    #region Edge Case Tests

    [Fact]
    public void Error_WithNullHttpContext_HandlesGracefully()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act
        var result = _controller.Error();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.NotNull(model);
    }

    [Fact]
    public void Error_WithEmptyTraceIdentifier_SetsRequestId()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = string.Empty;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = _controller.Error();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.NotNull(model.RequestId);
    }

    #endregion
}
