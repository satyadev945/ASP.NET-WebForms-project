using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Films.Web.Controllers;
using Films.Application.Services;
using Films.Application.DTOs;

namespace Films.Web.Tests.Controllers;

public class FilmsControllerTests
{
    private readonly Mock<IFilmService> _mockFilmService;
    private readonly Mock<ILogger<FilmsController>> _mockLogger;
    private readonly FilmsController _controller;

    public FilmsControllerTests()
    {
        _mockFilmService = new Mock<IFilmService>();
        _mockLogger = new Mock<ILogger<FilmsController>>();
        _controller = new FilmsController(_mockFilmService.Object, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenFilmServiceIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new FilmsController(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new FilmsController(_mockFilmService.Object, null!));
    }

    [Fact]
    public async Task Index_ShouldReturnViewResult()
    {
        var films = new List<FilmDto> { new FilmDto { Id = 1, Title = "Film 1", Year = 2020 } };
        _mockFilmService.Setup(s => s.GetAllFilmsAsync()).ReturnsAsync(films);

        var result = await _controller.Index();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Details_ShouldReturnViewResult_WithFilm()
    {
        var film = new FilmDto { Id = 1, Title = "Test Film", Year = 2020 };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(1)).ReturnsAsync(film);

        var result = await _controller.Details(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsType<FilmDto>(viewResult.Model);
    }

    [Fact]
    public async Task Details_ShouldReturnNotFound_WhenFilmDoesNotExist()
    {
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(999)).ReturnsAsync((FilmDto?)null);

        var result = await _controller.Details(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_Get_ShouldReturnViewResult()
    {
        var result = _controller.Create();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_Post_ShouldRedirectToIndex_WhenModelIsValid()
    {
        var createDto = new CreateFilmDto { Title = "New Film", Year = 2023 };
        var filmDto = new FilmDto { Id = 1, Title = "New Film", Year = 2023 };
        _mockFilmService.Setup(s => s.CreateFilmAsync(It.IsAny<CreateFilmDto>())).ReturnsAsync(filmDto);

        var result = await _controller.Create(createDto);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Edit_Get_ShouldReturnViewResult_WithUpdateDto()
    {
        var film = new FilmDto { Id = 1, Title = "Test Film", Year = 2020, Description = "Test", Genre = "Action" };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(1)).ReturnsAsync(film);

        var result = await _controller.Edit(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsType<UpdateFilmDto>(viewResult.Model);
    }

    [Fact]
    public async Task Edit_Post_ShouldRedirectToIndex_WhenModelIsValid()
    {
        var updateDto = new UpdateFilmDto { Id = 1, Title = "Updated Film", Year = 2023 };
        _mockFilmService.Setup(s => s.UpdateFilmAsync(It.IsAny<UpdateFilmDto>())).Returns(Task.CompletedTask);

        var result = await _controller.Edit(1, updateDto);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Edit_Post_ShouldReturnBadRequest_WhenIdMismatch()
    {
        var updateDto = new UpdateFilmDto { Id = 2 };

        var result = await _controller.Edit(1, updateDto);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Delete_Get_ShouldReturnViewResult_WithFilm()
    {
        var film = new FilmDto { Id = 1, Title = "Test Film", Year = 2020 };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(1)).ReturnsAsync(film);

        var result = await _controller.Delete(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsType<FilmDto>(viewResult.Model);
    }

    [Fact]
    public async Task DeleteConfirmed_ShouldRedirectToIndex()
    {
        _mockFilmService.Setup(s => s.DeleteFilmAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

        var result = await _controller.DeleteConfirmed(1);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }
}
