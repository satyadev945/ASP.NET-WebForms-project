using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Films.Web.Controllers;
using Films.Application.Services;
using Films.Application.DTOs;

namespace Films.Web.Tests.Controllers;

/// <summary>
/// Unit tests for FilmsController
/// </summary>
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

    #region Constructor Tests

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenFilmServiceIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new FilmsController(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new FilmsController(_mockFilmService.Object, null!));
    }

    [Fact]
    public void Constructor_ShouldCreateInstance_WhenParametersAreValid()
    {
        // Arrange, Act & Assert
        var controller = new FilmsController(_mockFilmService.Object, _mockLogger.Object);
        Assert.NotNull(controller);
    }

    #endregion

    #region Index Tests

    [Fact]
    public async Task Index_ShouldReturnViewWithFilms_WhenSuccessful()
    {
        // Arrange
        var films = new List<FilmDto>
        {
            new FilmDto { Id = 1, Title = "Film 1", Year = 2020 },
            new FilmDto { Id = 2, Title = "Film 2", Year = 2021 }
        };
        _mockFilmService.Setup(s => s.GetAllFilmsAsync()).ReturnsAsync(films);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<FilmDto>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Index_ShouldReturnViewWithEmptyList_WhenExceptionOccurs()
    {
        // Arrange
        _mockFilmService.Setup(s => s.GetAllFilmsAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<FilmDto>>(viewResult.Model);
        Assert.Empty(model);
        Assert.Equal("An error occurred while retrieving films.", _controller.TempData["Error"]);
    }

    [Fact]
    public async Task Index_ShouldLogError_WhenExceptionOccurs()
    {
        // Arrange
        _mockFilmService.Setup(s => s.GetAllFilmsAsync())
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        await _controller.Index();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task Index_ShouldReturnEmptyList_WhenNoFilmsExist()
    {
        // Arrange
        _mockFilmService.Setup(s => s.GetAllFilmsAsync()).ReturnsAsync(new List<FilmDto>());

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<FilmDto>>(viewResult.Model);
        Assert.Empty(model);
    }

    #endregion

    #region Details Tests

    [Fact]
    public async Task Details_ShouldReturnViewWithFilm_WhenFilmExists()
    {
        // Arrange
        var filmId = 1;
        var film = new FilmDto { Id = filmId, Title = "Test Film", Year = 2020 };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId)).ReturnsAsync(film);

        // Act
        var result = await _controller.Details(filmId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<FilmDto>(viewResult.Model);
        Assert.Equal(filmId, model.Id);
        Assert.Equal("Test Film", model.Title);
    }

    [Fact]
    public async Task Details_ShouldReturnNotFound_WhenFilmDoesNotExist()
    {
        // Arrange
        var filmId = 999;
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId)).ReturnsAsync((FilmDto?)null);

        // Act
        var result = await _controller.Details(filmId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_ShouldReturnNotFound_WhenExceptionOccurs()
    {
        // Arrange
        var filmId = 1;
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Details(filmId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_ShouldLogError_WhenExceptionOccurs()
    {
        // Arrange
        var filmId = 1;
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        await _controller.Details(filmId);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    #endregion

    #region Create Tests

    [Fact]
    public void Create_Get_ShouldReturnView()
    {
        // Act
        var result = _controller.Create();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_Post_ShouldRedirectToIndex_WhenModelIsValid()
    {
        // Arrange
        var createDto = new CreateFilmDto 
        { 
            Title = "New Film", 
            Year = 2023,
            .Throws(new Exception("Database error"));

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Film created successfully.", _controller.TempData["Success"]);
    }

    [Fact]
    public async Task Create_Post_ShouldReturnView_WhenModelStateIsInvalid()
    {
        // Arrange
        var createDto = new CreateFilmDto();
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(createDto, viewResult.Model);
    }

    [Fact]
    public async Task Create_Post_ShouldReturnView_WhenExceptionOccurs()
    {
        // Arrange
        var createDto = new CreateFilmDto 
        { 
            Title = "New Film", 
            Year = 2023 
        };
        _mockFilmService.Setup(s => s.CreateFilmAsync(createDto))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(createDto, viewResult.Model);
        Assert.False(_controller.ModelState.IsValid);
    }

    [Fact]
    public async Task Create_Post_ShouldLogError_WhenExceptionOccurs()
    {
        // Arrange
        var createDto = new CreateFilmDto { Title = "Test" };
        _mockFilmService.Setup(s => s.CreateFilmAsync(createDto))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        await _controller.Create(createDto);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    #endregion

    #region Edit Tests

    [Fact]
    public async Task Edit_Get_ShouldReturnViewWithFilm_WhenFilmExists()
    {
        // Arrange
        var filmId = 1;
        var film = new FilmDto 
        { 
            Id = filmId, 
            Title = "Test Film", 
            Year = 2020,
            Description = "Test description",
            Genre = "Action"
        };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId)).ReturnsAsync(film);

        // Act
        var result = await _controller.Edit(filmId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateFilmDto>(viewResult.Model);
        Assert.Equal(filmId, model.Id);
        Assert.Equal("Test Film", model.Title);
    }

    [Fact]
    public async Task Edit_Get_ShouldReturnNotFound_WhenFilmDoesNotExist()
    {
        // Arrange
        var filmId = 999;
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId)).ReturnsAsync((FilmDto?)null);

        // Act
        var result = await _controller.Edit(filmId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Get_ShouldReturnNotFound_WhenExceptionOccurs()
    {
        // Arrange
        var filmId = 1;
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Edit(filmId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Post_ShouldRedirectToIndex_WhenUpdateIsSuccessful()
    {
        // Arrange
        var filmId = 1;
        var updateDto = new UpdateFilmDto 
        { 
            Id = filmId, 
            Title = "Updated Film", 
            Year = 2023 
        };
        _mockFilmService.Setup(s => s.UpdateFilmAsync(updateDto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Edit(filmId, updateDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Film updated successfully.", _controller.TempData["Success"]);
    }

    [Fact]
    public async Task Edit_Post_ShouldReturnBadRequest_WhenIdMismatch()
    {
        // Arrange
        var filmId = 1;
        var updateDto = new UpdateFilmDto { Id = 2 };

        // Act
        var result = await _controller.Edit(filmId, updateDto);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Edit_Post_ShouldReturnView_WhenModelStateIsInvalid()
    {
        // Arrange
        var filmId = 1;
        var updateDto = new UpdateFilmDto { Id = filmId };
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = await _controller.Edit(filmId, updateDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(updateDto, viewResult.Model);
    }

    [Fact]
    public async Task Edit_Post_ShouldReturnView_WhenExceptionOccurs()
    {
        // Arrange
        var filmId = 1;
        var updateDto = new UpdateFilmDto { Id = filmId, Title = "Updated" };
        _mockFilmService.Setup(s => s.UpdateFilmAsync(updateDto))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Edit(filmId, updateDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(updateDto, viewResult.Model);
        Assert.False(_controller.ModelState.IsValid);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_Get_ShouldReturnViewWithFilm_WhenFilmExists()
    {
        // Arrange
        var filmId = 1;
        var film = new FilmDto { Id = filmId, Title = "Test Film", Year = 2020 };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId)).ReturnsAsync(film);

        // Act
        var result = await _controller.Delete(filmId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<FilmDto>(viewResult.Model);
        Assert.Equal(filmId, model.Id);
    }

    [Fact]
    public async Task Delete_Get_ShouldReturnNotFound_WhenFilmDoesNotExist()
    {
        // Arrange
        var filmId = 999;
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId)).ReturnsAsync((FilmDto?)null);

        // Act
        var result = await _controller.Delete(filmId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_Get_ShouldReturnNotFound_WhenExceptionOccurs()
    {
        // Arrange
        var filmId = 1;
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Delete(filmId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenDeleteIsSuccessful()
    {
        // Arrange
        var filmId = 1;
        _mockFilmService.Setup(s => s.DeleteFilmAsync(filmId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteConfirmed(filmId);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Film deleted successfully.", _controller.TempData["Success"]);
    }

    [Fact]
    public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenExceptionOccurs()
    {
        // Arrange
        var filmId = 1;
        _mockFilmService.Setup(s => s.DeleteFilmAsync(filmId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.DeleteConfirmed(filmId);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("An error occurred while deleting the film.", _controller.TempData["Error"]);
    }

    [Fact]
    public async Task DeleteConfirmed_ShouldLogError_WhenExceptionOccurs()
    {
        // Arrange
        var filmId = 1;
        _mockFilmService.Setup(s => s.DeleteFilmAsync(filmId))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        await _controller.DeleteConfirmed(filmId);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    #endregion
}
