using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Films.Web.Controllers;
using Films.Application.Services;
using Films.Application.DTOs;

namespace Films.Web.Tests.Controllers;

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
    public void Constructor_ShouldThrowArgumentNullException_WhenActorServiceIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new ActorsController(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new ActorsController(_mockActorService.Object, null!));
    }

    [Fact]
    public async Task Index_ShouldReturnViewResult()
    {
        var actors = new List<ActorDto> { new ActorDto { Id = 1, FirstName = "John", LastName = "Doe" } };
        _mockActorService.Setup(s => s.GetAllActorsAsync()).ReturnsAsync(actors);

        var result = await _controller.Index();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Details_ShouldReturnViewResult_WithActor()
    {
        var actor = new ActorDto { Id = 1, FirstName = "John", LastName = "Doe" };
        _mockActorService.Setup(s => s.GetActorByIdAsync(1)).ReturnsAsync(actor);

        var result = await _controller.Details(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsType<ActorDto>(viewResult.Model);
    }

    [Fact]
    public async Task Details_ShouldReturnNotFound_WhenActorDoesNotExist()
    {
        _mockActorService.Setup(s => s.GetActorByIdAsync(999)).ReturnsAsync((ActorDto?)null);

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
        var createDto = new CreateActorDto { FirstName = "John", LastName = "Doe" };
        var actorDto = new ActorDto { Id = 1, FirstName = "John", LastName = "Doe" };
        _mockActorService.Setup(s => s.CreateActorAsync(It.IsAny<CreateActorDto>())).ReturnsAsync(actorDto);

        var result = await _controller.Create(createDto);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Edit_Get_ShouldReturnViewResult_WithUpdateDto()
    {
        var actor = new ActorDto { Id = 1, FirstName = "John", LastName = "Doe", SexId = 1, BirthDate = DateTime.Now };
        _mockActorService.Setup(s => s.GetActorByIdAsync(1)).ReturnsAsync(actor);

        var result = await _controller.Edit(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsType<UpdateActorDto>(viewResult.Model);
    }

    [Fact]
    public async Task Edit_Post_ShouldRedirectToIndex_WhenModelIsValid()
    {
        var updateDto = new UpdateActorDto { Id = 1, FirstName = "John", LastName = "Doe" };
        _mockActorService.Setup(s => s.UpdateActorAsync(It.IsAny<UpdateActorDto>())).Returns(Task.CompletedTask);

        var result = await _controller.Edit(1, updateDto);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Edit_Post_ShouldReturnBadRequest_WhenIdMismatch()
    {
        var updateDto = new UpdateActorDto { Id = 2 };

        var result = await _controller.Edit(1, updateDto);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Delete_Get_ShouldReturnViewResult_WithActor()
    {
        var actor = new ActorDto { Id = 1, FirstName = "John", LastName = "Doe" };
        _mockActorService.Setup(s => s.GetActorByIdAsync(1)).ReturnsAsync(actor);

        var result = await _controller.Delete(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsType<ActorDto>(viewResult.Model);
    }

    [Fact]
    public async Task DeleteConfirmed_ShouldRedirectToIndex()
    {
        _mockActorService.Setup(s => s.DeleteActorAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

        var result = await _controller.DeleteConfirmed(1);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }
}
