using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Films.Web.Controllers;
using Films.Application.Services;
using Films.Application.DTOs;

namespace Films.Web.Tests.Controllers;

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
    public void Constructor_ShouldThrowArgumentNullException_WhenActorServiceIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new ActorsController(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new ActorsController(_mockActorService.Object, null!));
    }

    [Fact]
    public void Constructor_ShouldCreateInstance_WhenParametersAreValid()
    {
        // Arrange, Act & Assert
        var controller = new ActorsController(_mockActorService.Object, _mockLogger.Object);
        Assert.NotNull(controller);
    }

    #endregion

    #region Index Tests

    [Fact]
    public async Task Index_ShouldReturnViewWithActors_WhenSuccessful()
    {
        // Arrange
        var actors = new List<ActorDto>
        {
            new ActorDto { Id = 1, FirstName = "John", LastName = "Doe" },
            new ActorDto { Id = 2, FirstName = "Jane", LastName = "Smith" }
        };
        _mockActorService.Setup(s => s.GetAllActorsAsync()).ReturnsAsync(actors);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<ActorDto>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Index_ShouldReturnViewWithEmptyList_WhenExceptionOccurs()
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
        Assert.Equal("An error occurred while retrieving actors.", _controller.TempData["Error"]);
    }

    [Fact]
    public async Task Index_ShouldLogError_WhenExceptionOccurs()
    {
        // Arrange
        _mockActorService.Setup(s => s.GetAllActorsAsync())
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

    #endregion

    #region Details Tests

    [Fact]
    public async Task Details_ShouldReturnViewWithActor_WhenActorExists()
    {
        // Arrange
        var actorId = 1;
        var actor = new ActorDto { Id = actorId, FirstName = "John", LastName = "Doe" };
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId)).ReturnsAsync(actor);

        // Act
        var result = await _controller.Details(actorId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ActorDto>(viewResult.Model);
        Assert.Equal(actorId, model.Id);
    }

    [Fact]
    public async Task Details_ShouldReturnNotFound_WhenActorDoesNotExist()
    {
        // Arrange
        var actorId = 999;
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId)).ReturnsAsync((ActorDto?)null);

        // Act
        var result = await _controller.Details(actorId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_ShouldReturnNotFound_WhenExceptionOccurs()
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
        var createDto = new CreateActorDto 
        { 
            FirstName = "John", 
            LastName = "Doe",
            .Throws(new Exception("Database error"));

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Actor created successfully.", _controller.TempData["Success"]);
    }

    [Fact]
    public async Task Create_Post_ShouldReturnView_WhenModelStateIsInvalid()
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
    public async Task Create_Post_ShouldReturnView_WhenExceptionOccurs()
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
        Assert.Equal(createDto, viewResult.Model);
        Assert.False(_controller.ModelState.IsValid);
    }

    #endregion

    #region Edit Tests

    [Fact]
    public async Task Edit_Get_ShouldReturnViewWithActor_WhenActorExists()
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
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId)).ReturnsAsync(actor);

        // Act
        var result = await _controller.Edit(actorId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateActorDto>(viewResult.Model);
        Assert.Equal(actorId, model.Id);
        Assert.Equal("John", model.FirstName);
    }

    [Fact]
    public async Task Edit_Get_ShouldReturnNotFound_WhenActorDoesNotExist()
    {
        // Arrange
        var actorId = 999;
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId)).ReturnsAsync((ActorDto?)null);

        // Act
        var result = await _controller.Edit(actorId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Post_ShouldRedirectToIndex_WhenUpdateIsSuccessful()
    {
        // Arrange
        var actorId = 1;
        var updateDto = new UpdateActorDto 
        { 
            Id = actorId, 
            FirstName = "John", 
            LastName = "Doe" 
        };
        _mockActorService.Setup(s => s.UpdateActorAsync(updateDto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Edit(actorId, updateDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Actor updated successfully.", _controller.TempData["Success"]);
    }

    [Fact]
    public async Task Edit_Post_ShouldReturnBadRequest_WhenIdMismatch()
    {
        // Arrange
        var actorId = 1;
        var updateDto = new UpdateActorDto { Id = 2 };

        // Act
        var result = await _controller.Edit(actorId, updateDto);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Edit_Post_ShouldReturnView_WhenModelStateIsInvalid()
    {
        // Arrange
        var actorId = 1;
        var updateDto = new UpdateActorDto { Id = actorId };
        _controller.ModelState.AddModelError("FirstName", "Required");

        // Act
        var result = await _controller.Edit(actorId, updateDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(updateDto, viewResult.Model);
    }

    [Fact]
    public async Task Edit_Post_ShouldReturnView_WhenExceptionOccurs()
    {
        // Arrange
        var actorId = 1;
        var updateDto = new UpdateActorDto { Id = actorId, FirstName = "John" };
        _mockActorService.Setup(s => s.UpdateActorAsync(updateDto))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Edit(actorId, updateDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(updateDto, viewResult.Model);
        Assert.False(_controller.ModelState.IsValid);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_Get_ShouldReturnViewWithActor_WhenActorExists()
    {
        // Arrange
        var actorId = 1;
        var actor = new ActorDto { Id = actorId, FirstName = "John", LastName = "Doe" };
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId)).ReturnsAsync(actor);

        // Act
        var result = await _controller.Delete(actorId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ActorDto>(viewResult.Model);
        Assert.Equal(actorId, model.Id);
    }

    [Fact]
    public async Task Delete_Get_ShouldReturnNotFound_WhenActorDoesNotExist()
    {
        // Arrange
        var actorId = 999;
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId)).ReturnsAsync((ActorDto?)null);

        // Act
        var result = await _controller.Delete(actorId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenDeleteIsSuccessful()
    {
        // Arrange
        var actorId = 1;
        _mockActorService.Setup(s => s.DeleteActorAsync(actorId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteConfirmed(actorId);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Actor deleted successfully.", _controller.TempData["Success"]);
    }

    [Fact]
    public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenExceptionOccurs()
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
        Assert.Equal("An error occurred while deleting the actor.", _controller.TempData["Error"]);
    }

    #endregion
}
