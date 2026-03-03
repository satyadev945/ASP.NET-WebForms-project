using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Films.Web.Controllers;
using Films.Application.Services;
using Films.Application.DTOs;

namespace Films.Web.Controllers.Tests;

/// <summary>
/// Unit tests for ActorsController
/// </summary>
public class ActorsControllerTests
{
    private readonly Mock<IActorService> _mockActorService;
    private readonly Mock<ILogger<ActorsController>> _mockLogger;
    private readonly ActorsController _controller;

    public ActorsControllerTests()
    {
        _mockActorService = new Mock<IActorService>();
        _mockLogger = new Mock<ILogger<ActorsController>>();
        _controller = new ActorsController(_mockActorService.Object, _mockLogger.Object);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance()
    {
        // Act
        var controller = new ActorsController(_mockActorService.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(controller);
    }

    [Fact]
    public void Constructor_WithNullActorService_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new ActorsController(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new ActorsController(_mockActorService.Object, null!));
    }

    #endregion

    #region Index Tests

    [Fact]
    public async Task Index_ReturnsViewResult_WithListOfActors()
    {
        // Arrange
        var actors = new List<ActorDto>
        {
            new ActorDto { Id = 1, FirstName = "John", LastName = "Doe" },
            new ActorDto { Id = 2, FirstName = "Jane", LastName = "Smith" }
        };
        _mockActorService.Setup(s => s.GetAllActorsAsync())
            .ReturnsAsync(actors);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<ActorDto>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Index_WhenServiceThrowsException_ReturnsViewWithEmptyList()
    {
        // Arrange
        _mockActorService.Setup(s => s.GetAllActorsAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<ActorDto>>(viewResult.Model);
        Assert.Empty(model);
    }

    [Fact]
    public async Task Index_WhenServiceReturnsEmptyList_ReturnsViewWithEmptyList()
    {
        // Arrange
        _mockActorService.Setup(s => s.GetAllActorsAsync())
            .ReturnsAsync(new List<ActorDto>());

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<ActorDto>>(viewResult.Model);
        Assert.Empty(model);
    }

    #endregion

    #region Details Tests

    [Fact]
    public async Task Details_WithValidId_ReturnsViewResult()
    {
        // Arrange
        var actorId = 1;
        var actor = new ActorDto { Id = actorId, FirstName = "John", LastName = "Doe" };
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId))
            .ReturnsAsync(actor);

        // Act
        var result = await _controller.Details(actorId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ActorDto>(viewResult.Model);
        Assert.Equal(actorId, model.Id);
    }

    [Fact]
    public async Task Details_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var actorId = 999;
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId))
            .ReturnsAsync((ActorDto?)null);

        // Act
        var result = await _controller.Details(actorId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_WhenServiceThrowsException_ReturnsNotFound()
    {
        // Arrange
        var actorId = 1;
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Details(actorId);

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
        var createDto = new CreateActorDto 
        { 
            FirstName = "John", 
            LastName = "Doe",
            SexId = 1,
            BirthDate = new DateTime(1980, 1, 1)
        };
        _mockActorService.Setup(s => s.CreateActorAsync(createDto))
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
        var createDto = new CreateActorDto();
        _controller.ModelState.AddModelError("FirstName", "Required");

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
        var createDto = new CreateActorDto 
        { 
            FirstName = "John", 
            LastName = "Doe" 
        };
        _mockActorService.Setup(s => s.CreateActorAsync(createDto))
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
        var actorId = 1;
        var actor = new ActorDto 
        { 
            Id = actorId, 
            FirstName = "John", 
            LastName = "Doe",
            SexId = 1,
            BirthDate = new DateTime(1980, 1, 1)
        };
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId))
            .ReturnsAsync(actor);

        // Act
        var result = await _controller.Edit(actorId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateActorDto>(viewResult.Model);
        Assert.Equal(actorId, model.Id);
    }

    [Fact]
    public async Task Edit_Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var actorId = 999;
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId))
            .ReturnsAsync((ActorDto?)null);

        // Act
        var result = await _controller.Edit(actorId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Post_WithValidModel_RedirectsToIndex()
    {
        // Arrange
        var updateDto = new UpdateActorDto 
        { 
            Id = 1, 
            FirstName = "John", 
            LastName = "Doe",
            SexId = 1,
            BirthDate = new DateTime(1980, 1, 1)
        };
        _mockActorService.Setup(s => s.UpdateActorAsync(updateDto))
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
        var updateDto = new UpdateActorDto { Id = 2 };

        // Act
        var result = await _controller.Edit(1, updateDto);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Edit_Post_WithInvalidModel_ReturnsViewWithModel()
    {
        // Arrange
        var updateDto = new UpdateActorDto { Id = 1 };
        _controller.ModelState.AddModelError("FirstName", "Required");

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
        var updateDto = new UpdateActorDto 
        { 
            Id = 1, 
            FirstName = "John", 
            LastName = "Doe" 
        };
        _mockActorService.Setup(s => s.UpdateActorAsync(updateDto))
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
        var actorId = 1;
        var actor = new ActorDto { Id = actorId, FirstName = "John", LastName = "Doe" };
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId))
            .ReturnsAsync(actor);

        // Act
        var result = await _controller.Delete(actorId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ActorDto>(viewResult.Model);
        Assert.Equal(actorId, model.Id);
    }

    [Fact]
    public async Task Delete_Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var actorId = 999;
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId))
            .ReturnsAsync((ActorDto?)null);

        // Act
        var result = await _controller.Delete(actorId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_WithValidId_RedirectsToIndex()
    {
        // Arrange
        var actorId = 1;
        _mockActorService.Setup(s => s.DeleteActorAsync(actorId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteConfirmed(actorId);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task DeleteConfirmed_WhenServiceThrowsException_RedirectsToIndex()
    {
        // Arrange
        var actorId = 1;
        _mockActorService.Setup(s => s.DeleteActorAsync(actorId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.DeleteConfirmed(actorId);

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
        _mockActorService.Setup(s => s.GetActorByIdAsync(0))
            .ReturnsAsync((ActorDto?)null);

        // Act
        var result = await _controller.Details(0);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_WithNegativeId_ReturnsNotFound()
    {
        // Arrange
        _mockActorService.Setup(s => s.GetActorByIdAsync(-1))
            .ReturnsAsync((ActorDto?)null);

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

    #endregion
}
