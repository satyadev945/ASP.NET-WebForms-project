using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Films.Web.Controllers;
using Films.Application.Services;
using Films.Application.DTOs;

namespace Films.Tests.Unit.Web.Controllers;

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

    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance()
    {
        var controller = new ActorsController(_mockActorService.Object, _mockLogger.Object);
        Assert.NotNull(controller);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithListOfActors()
    {
        var actors = new List<ActorDto>
        {
            new ActorDto { Id = 1, FirstName = "John", LastName = "Doe" },
            new ActorDto { Id = 2, FirstName = "Jane", LastName = "Smith" }
        };
        _mockActorService.Setup(s => s.GetAllActorsAsync()).ReturnsAsync(actors);

        var result = await _controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<ActorDto>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Index_WhenServiceThrowsException_ReturnsViewWithEmptyList()
    {
        _mockActorService.Setup(s => s.GetAllActorsAsync()).ThrowsAsync(new Exception("Database error"));

        var result = await _controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<ActorDto>>(viewResult.Model);
        Assert.Empty(model);
    }

    [Fact]
    public async Task Details_WithValidId_ReturnsViewResult()
    {
        var actorId = 1;
        var actor = new ActorDto { Id = actorId, FirstName = "John", LastName = "Doe" };
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId)).ReturnsAsync(actor);

        var result = await _controller.Details(actorId);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ActorDto>(viewResult.Model);
        Assert.Equal(actorId, model.Id);
    }

    [Fact]
    public async Task Details_WithInvalidId_ReturnsNotFound()
    {
        var actorId = 999;
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId)).ReturnsAsync((ActorDto?)null);

        var result = await _controller.Details(actorId);

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
        var createDto = new CreateActorDto 
        { 
            FirstName = "John", 
            LastName = "Doe",
            SexId = 1,
            BirthDate = new DateTime(1980, 1, 1)
        };
        _mockActorService.Setup(s => s.CreateActorAsync(createDto)).Returns(() => Task.CompletedTask);

        var result = await _controller.Create(createDto);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Create_Post_WithInvalidModel_ReturnsViewWithModel()
    {
        var createDto = new CreateActorDto();
        _controller.ModelState.AddModelError("FirstName", "Required");

        var result = await _controller.Create(createDto);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(createDto, viewResult.Model);
    }

    [Fact]
    public async Task Edit_Get_WithValidId_ReturnsViewWithModel()
    {
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

        var result = await _controller.Edit(actorId);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UpdateActorDto>(viewResult.Model);
        Assert.Equal(actorId, model.Id);
    }

    [Fact]
    public async Task Edit_Post_WithValidModel_RedirectsToIndex()
    {
        var updateDto = new UpdateActorDto 
        { 
            Id = 1, 
            FirstName = "John", 
            LastName = "Doe",
            SexId = 1,
            BirthDate = new DateTime(1980, 1, 1)
        };
        _mockActorService.Setup(s => s.UpdateActorAsync(updateDto)).Returns(() => Task.CompletedTask);

        var result = await _controller.Edit(1, updateDto);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Edit_Post_WithMismatchedId_ReturnsBadRequest()
    {
        var updateDto = new UpdateActorDto { Id = 2 };

        var result = await _controller.Edit(1, updateDto);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Delete_Get_WithValidId_ReturnsViewWithModel()
    {
        var actorId = 1;
        var actor = new ActorDto { Id = actorId, FirstName = "John", LastName = "Doe" };
        _mockActorService.Setup(s => s.GetActorByIdAsync(actorId)).ReturnsAsync(actor);

        var result = await _controller.Delete(actorId);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ActorDto>(viewResult.Model);
        Assert.Equal(actorId, model.Id);
    }

    [Fact]
    public async Task DeleteConfirmed_WithValidId_RedirectsToIndex()
    {
        var actorId = 1;
        _mockActorService.Setup(s => s.DeleteActorAsync(actorId)).Returns(() => Task.CompletedTask);

        var result = await _controller.DeleteConfirmed(actorId);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }
}
