using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Films.Web.Controllers;
using Films.Application.Services;
using Films.Application.DTOs;

namespace Films.Web.Controllers.Tests;

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
    public void Constructor_WithValidParameters_CreatesInstance()
    {
        // Act
        var controller = new FilmsController(_mockFilmService.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(controller);
    }

    [Fact]
    public void Constructor_WithNullFilmService_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new FilmsController(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new FilmsController(_mockFilmService.Object, null!));
    }

    #endregion

    #region Index Tests

    [Fact]
    public async Task Index_ReturnsViewResult_WithListOfFilms()
    {
        // Arrange
        var films = new List<FilmDto>
        {
            new FilmDto { Id = 1, Title = "Film 1", Year = 2020 },
            new FilmDto { Id = 2, Title = "Film 2", Year = 2021 }
        };
        _mockFilmService.Setup(s => s.GetAllFilmsAsync())
            .ReturnsAsync(films);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<FilmDto>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Index_WhenServiceThrowsException_ReturnsViewWithEmptyList()
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
    }

    [Fact]
    public async Task Index_WhenServiceReturnsEmptyList_ReturnsViewWithEmptyList()
    {
        // Arrange
        _mockFilmService.Setup(s => s.GetAllFilmsAsync())
            .ReturnsAsync(new List<FilmDto>());

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
    public async Task Details_WithValidId_ReturnsViewResult()
    {
        // Arrange
        var filmId = 1;
        var film = new FilmDto { Id = filmId, Title = "Test Film", Year = 2020 };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId))
            .ReturnsAsync(film);

        // Act
        var result = await _controller.Details(filmId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<FilmDto>(viewResult.Model);
        Assert.Equal(filmId, model.Id);
    }

    [Fact]
    public async Task Details_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var filmId = 999;
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId))
            .ReturnsAsync((FilmDto?)null);

        // Act
        var result = await _controller.Details(filmId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_WhenServiceThrowsException_ReturnsNotFound()
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

    #endregion

    #region Create Tests

    [Fact]
    public void Create_Get_ReturnsViewResult()
    {
        // Act
        var result = _controller.Create();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_Post_WithValidModel_RedirectsToIndex()
    {
        // Arrange
        var createDto = new CreateFilmDto 
        { 
            Title = "New Film", 
            Description = "Description",
            Year = 2023,
            Genre = "Action"
        };
        _mockFilmService.Setup(s => s.CreateFilmAsync(createDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Create_Post_WithInvalidModel_ReturnsViewWithModel()
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
    public async Task Create_Post_WhenServiceThrowsException_ReturnsViewWithError()
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
        Assert.False(_controller.ModelState.IsValid);
    }

    #endregion

    #region Edit Tests

    [Fact]
    public async Task Edit_Get_WithValidId_ReturnsViewWithModel()
    {
        // Arrange
        var filmId = 1;
        var film = new FilmDto 
        { 
            Id = filmId, 
            Title = "Test Film", 
            Description = "Description",
            Year = 2020,
            Genre = "Drama"
        };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId))
            .ReturnsAsync(film);

        // Act
        var result = await _controller.Edit(filmId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateFilmDto>(viewResult.Model);
        Assert.Equal(filmId, model.Id);
    }

    [Fact]
    public async Task Edit_Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var filmId = 999;
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId))
            .ReturnsAsync((FilmDto?)null);

        // Act
        var result = await _controller.Edit(filmId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Post_WithValidModel_RedirectsToIndex()
    {
        // Arrange
        var updateDto = new UpdateFilmDto 
        { 
            Id = 1, 
            Title = "Updated Film", 
            Description = "Updated Description",
            Year = 2023,
            Genre = "Action"
        };
        _mockFilmService.Setup(s => s.UpdateFilmAsync(updateDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Edit(1, updateDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Edit_Post_WithMismatchedId_ReturnsBadRequest()
    {
        // Arrange
        var updateDto = new UpdateFilmDto { Id = 2 };

        // Act
        var result = await _controller.Edit(1, updateDto);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Edit_Post_WithInvalidModel_ReturnsViewWithModel()
    {
        // Arrange
        var updateDto = new UpdateFilmDto { Id = 1 };
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = await _controller.Edit(1, updateDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(updateDto, viewResult.Model);
    }

    [Fact]
    public async Task Edit_Post_WhenServiceThrowsException_ReturnsViewWithError()
    {
        // Arrange
        var updateDto = new UpdateFilmDto 
        { 
            Id = 1, 
            Title = "Updated Film", 
            Year = 2023 
        };
        _mockFilmService.Setup(s => s.UpdateFilmAsync(updateDto))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Edit(1, updateDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_Get_WithValidId_ReturnsViewWithModel()
    {
        // Arrange
        var filmId = 1;
        var film = new FilmDto { Id = filmId, Title = "Test Film", Year = 2020 };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId))
            .ReturnsAsync(film);

        // Act
        var result = await _controller.Delete(filmId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<FilmDto>(viewResult.Model);
        Assert.Equal(filmId, model.Id);
    }

    [Fact]
    public async Task Delete_Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var filmId = 999;
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId))
            .ReturnsAsync((FilmDto?)null);

        // Act
        var result = await _controller.Delete(filmId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_WithValidId_RedirectsToIndex()
    {
        // Arrange
        var filmId = 1;
        _mockFilmService.Setup(s => s.DeleteFilmAsync(filmId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteConfirmed(filmId);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task DeleteConfirmed_WhenServiceThrowsException_RedirectsToIndex()
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
    }

    #endregion

    #region Edge Case Tests

    [Fact]
    public async Task Details_WithZeroId_ReturnsNotFound()
    {
        // Arrange
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(0))
            .ReturnsAsync((FilmDto?)null);

        // Act
        var result = await _controller.Details(0);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_WithNegativeId_ReturnsNotFound()
    {
        // Arrange
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(-1))
            .ReturnsAsync((FilmDto?)null);

        // Act
        var result = await _controller.Details(-1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_Post_WithNullDto_ThrowsException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(async () => 
            await _controller.Create(null!));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(999)]
    public async Task Details_WithVariousValidIds_CallsService(int filmId)
    {
        // Arrange
        var film = new FilmDto { Id = filmId, Title = "Test Film" };
        _mockFilmService.Setup(s => s.GetFilmByIdAsync(filmId))
            .ReturnsAsync(film);

        // Act
        await _controller.Details(filmId);

        // Assert
        _mockFilmService.Verify(s => s.GetFilmByIdAsync(filmId), Times.Once);
    }

    #endregion
}
