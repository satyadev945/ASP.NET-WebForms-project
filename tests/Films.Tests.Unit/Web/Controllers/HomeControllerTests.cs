using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Films.Web.Controllers;
using Films.Web.Models;
using Microsoft.AspNetCore.Http;

namespace Films.Tests.Unit.Web.Controllers;

public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _mockLogger;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _mockLogger = new Mock<ILogger<HomeController>>();
        _controller = new HomeController(_mockLogger.Object);
    }

    [Fact]
    public void Index_ReturnsViewResult()
    {
        var result = _controller.Index();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void About_ReturnsViewResult()
    {
        var result = _controller.About();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Contact_ReturnsViewResult()
    {
        var result = _controller.Contact();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Privacy_ReturnsViewResult()
    {
        var result = _controller.Privacy();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error_ReturnsViewResult()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = "test-trace-id";
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        
        var result = _controller.Error();
        
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error_ReturnsViewWithErrorViewModel()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = "test-trace-id";
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        
        var result = _controller.Error();
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.NotNull(model);
    }

    [Fact]
    public void Error_SetsRequestIdFromTraceIdentifier()
    {
        var expectedTraceId = "test-trace-id-123";
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = expectedTraceId;
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        
        var result = _controller.Error();
        
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.Equal(expectedTraceId, model.RequestId);
    }
}
