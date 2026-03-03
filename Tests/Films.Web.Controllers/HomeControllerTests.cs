using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Films.Web.Controllers;
using Films.Web.Models;
using System.Diagnostics;

namespace Films.Web.Tests.Controllers;

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
    public void Constructor_ShouldCreateInstance_WhenLoggerIsProvided()
    {
        // Arrange, Act & Assert
        var controller = new HomeController(_mockLogger.Object);
        Assert.NotNull(controller);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new HomeController(null!));
    }

    #endregion

    #region Index Tests

    [Fact]
    public void Index_ShouldReturnViewResult()
    {
        // Act
        var result = _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Index_ShouldLogInformation()
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
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public void Index_ShouldReturnViewWithNoModel()
    {
        // Act
        var result = _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Model);
    }

    #endregion

    #region About Tests

    [Fact]
    public void About_ShouldReturnViewResult()
    {
        // Act
        var result = _controller.About();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void About_ShouldLogInformation()
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
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public void About_ShouldReturnViewWithNoModel()
    {
        // Act
        var result = _controller.About() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Model);
    }

    #endregion

    #region Contact Tests

    [Fact]
    public void Contact_ShouldReturnViewResult()
    {
        // Act
        var result = _controller.Contact();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Contact_ShouldLogInformation()
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
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public void Contact_ShouldReturnViewWithNoModel()
    {
        // Act
        var result = _controller.Contact() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Model);
    }

    #endregion

    #region Privacy Tests

    [Fact]
    public void Privacy_ShouldReturnViewResult()
    {
        // Act
        var result = _controller.Privacy();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Privacy_ShouldReturnViewWithNoModel()
    {
        // Act
        var result = _controller.Privacy() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Model);
    }

    [Fact]
    public void Privacy_ShouldNotLogInformation()
    {
        // Act
        _controller.Privacy();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    #endregion

    #region Error Tests

    [Fact]
    public void Error_ShouldReturnViewResult()
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
    public void Error_ShouldReturnViewWithErrorViewModel()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = "test-trace-id";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = _controller.Error() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsType<ErrorViewModel>(result.Model);
        Assert.NotNull(model);
    }

    [Fact]
    public void Error_ShouldSetRequestIdFromTraceIdentifier_WhenActivityCurrentIsNull()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = "test-trace-id-123";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = _controller.Error() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsType<ErrorViewModel>(result.Model);
        Assert.Equal("test-trace-id-123", model.RequestId);
    }

    [Fact]
    public void Error_ShouldHaveResponseCacheAttribute()
    {
        // Arrange
        var methodInfo = typeof(HomeController).GetMethod("Error");

        // Act
        var attributes = methodInfo?.GetCustomAttributes(typeof(ResponseCacheAttribute), false);

        // Assert
        Assert.NotNull(attributes);
        Assert.Single(attributes);
        var attribute = attributes[0] as ResponseCacheAttribute;
        Assert.NotNull(attribute);
        Assert.Equal(0, attribute.Duration);
        Assert.Equal(ResponseCacheLocation.None, attribute.Location);
        Assert.True(attribute.NoStore);
    }

    [Fact]
    public void Error_ShouldReturnViewWithShowRequestIdTrue_WhenRequestIdIsSet()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = "valid-trace-id";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = _controller.Error() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsType<ErrorViewModel>(result.Model);
        Assert.True(model.ShowRequestId);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void AllActions_ShouldReturnViewResult()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act & Assert
        Assert.IsType<ViewResult>(_controller.Index());
        Assert.IsType<ViewResult>(_controller.About());
        Assert.IsType<ViewResult>(_controller.Contact());
        Assert.IsType<ViewResult>(_controller.Privacy());
        Assert.IsType<ViewResult>(_controller.Error());
    }

    [Fact]
    public void LoggingActions_ShouldLogCorrectMessages()
    {
        // Act
        _controller.Index();
        _controller.About();
        _controller.Contact();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Exactly(3));
    }

    #endregion
}
