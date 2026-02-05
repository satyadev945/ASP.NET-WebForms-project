using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Web.Controllers;
using Films.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Films.Web.Controllers.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<IFilmService> _mockFilmService;
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockFilmService = new Mock<IFilmService>();
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockFilmService.Object, _mockLogger.Object);

            // Setup HttpContext for traceIdentifier
            var httpContext = new DefaultHttpContext();
            httpContext.TraceIdentifier = "test-trace-id";
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task Index_ReturnsViewWithListOfFilms()
        {
            // Arrange
            var films = new List<Film>
            {
                new Film { Id = 1, Name = "Inception", Year = 2010 },
                new Film { Id = 2, Name = "The Shawshank Redemption", Year = 1994 }
            };
            _mockFilmService.Setup(s => s.GetAllFilmsAsync()).ReturnsAsync(films);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Film>>(viewResult.Model);
            Assert.Equal(2, ((List<Film>)model).Count);
        }

        [Fact]
        public async Task Index_WhenExceptionOccurs_ReturnsErrorView()
        {
            // Arrange
            _mockFilmService.Setup(s => s.GetAllFilmsAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.Equal("test-trace-id", model.RequestId);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void Privacy_ReturnsView()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void About_ReturnsView()
        {
            // Act
            var result = _controller.About();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Contact_ReturnsView()
        {
            // Act
            var result = _controller.Contact();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewWithErrorViewModel()
        {
            // Act
            var result = _controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.Equal("test-trace-id", model.RequestId);
        }
    }
}