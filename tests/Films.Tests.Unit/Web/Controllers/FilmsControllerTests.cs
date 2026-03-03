using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Films.Web.Controllers;
using Films.Application.Services;
using Films.Application.DTOs;

namespace Films.Tests.Unit.Web.Controllers;

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
    public void Constructor_WithValidParameters_CreatesInstance()
    {
        var controller = new FilmsController(_mockFilmService.Object, _mockLogger.Object);
        Assert.NotNull(controller);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithListOfFilms()
    {
        var films = new List<FilmDto>
        {
            new FilmDto { Id = 1, Title = "Film 1", Year = 2020 },
            new FilmDto { Id = 2, Title = "Film 2", Year = 2021 }
        };
        _mockFilmService.Setup(s => s.GetAllFilmsAsync()).ReturnsAsync(films);

        var result = await _controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<FilmDto>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Index_WhenServiceThrowsException_ReturnsViewWithEmptyList()
    {
        _mockFilmService.Setup(s => s.GetAllFilmsAsync()).ThrowsAsync(new Exception("Database error"));

        var result = await _controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<FilmDto>>(viewResult.Model);
        Assert.Empty(model);
    }

    [Fact]
    public async Task Details_WithValidId_ReturnsViewResult()
    {
        var filmId = 1;
        var film = new FilmDto { Id = filmId, Title = "Test Film", Year = 2020 };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId)).ReturnsAsync(film);

        var result = await _controller.Details(filmId);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<FilmDto>(viewResult.Model);
        Assert.Equal(filmId, model.Id);
    }

    [Fact]
    public async Task Details_WithInvalidId_ReturnsNotFound()
    {
        var filmId = 999;
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId)).ReturnsAsync((FilmDto?)null);

        var result = await _controller.Details(filmId);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_Get_ReturnsViewResult()
    {
        var result = _controller.Create();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_Post_WithValidModel_RedirectsToIndex()
    {
        var createDto = new CreateFilmDto 
        { 
            Title = "New Film", 
            Description = "Description",
            Year = 2023,
            Genre = "Action"
        };
        _mockFilmService.Setup(s => s.CreateFilmAsync(createDto)).Returns(() => Task.CompletedTask);

        var result = await _controller.Create(createDto);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Create_Post_WithInvalidModel_ReturnsViewWithModel()
    {
        var createDto = new CreateFilmDto();
        _controller.ModelState.AddModelError("Title", "Required");

        var result = await _controller.Create(createDto);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(createDto, viewResult.Model);
    }

    [Fact]
    public async Task Edit_Get_WithValidId_ReturnsViewWithModel()
    {
        var filmId = 1;
        var film = new FilmDto 
        { 
            Id = filmId, 
            Title = "Test Film", 
            Description = "Description",
            Year = 2020,
            Genre = "Drama"
        };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId)).ReturnsAsync(film);

        var result = await _controller.Edit(filmId);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateFilmDto>(viewResult.Model);
        Assert.Equal(filmId, model.Id);
    }

    [Fact]
    public async Task Edit_Post_WithValidModel_RedirectsToIndex()
    {
        var updateDto = new UpdateFilmDto 
        { 
            Id = 1, 
            Title = "Updated Film", 
            Description = "Updated Description",
            Year = 2023,
            Genre = "Action"
        };
        _mockFilmService.Setup(s => s.UpdateFilmAsync(updateDto)).Returns(() => Task.CompletedTask);

        var result = await _controller.Edit(1, updateDto);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Edit_Post_WithMismatchedId_ReturnsBadRequest()
    {
        var updateDto = new UpdateFilmDto { Id = 2 };

        var result = await _controller.Edit(1, updateDto);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Delete_Get_WithValidId_ReturnsViewWithModel()
    {
        var filmId = 1;
        var film = new FilmDto { Id = filmId, Title = "Test Film", Year = 2020 };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId)).ReturnsAsync(film);

        var result = await _controller.Delete(filmId);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<FilmDto>(viewResult.Model);
        Assert.Equal(filmId, model.Id);
    }

    [Fact]
    public async Task DeleteConfirmed_WithValidId_RedirectsToIndex()
    {
        var filmId = 1;
        _mockFilmService.Setup(s => s.DeleteFilmAsync(filmId)).Returns(() => Task.CompletedTask);

        var result = await _controller.DeleteConfirmed(filmId);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }
}
