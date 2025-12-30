using AutoMapper;
using Films.Application.DTOs;
using Films.Application.Services;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Films.UnitTests.Services;

public class ActorServiceTests
{
    private readonly Mock<IActorRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<ActorService>> _mockLogger;
    private readonly ActorService _service;

    public ActorServiceTests()
    {
        _mockRepository = new Mock<IActorRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<ActorService>>();
        _service = new ActorService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllActors_WhenActorsExist()
    {
        // Arrange
        var actors = new List<Actor>
        {
            new Actor { Id = 1, Name = "Actor 1", IsActive = true },
            new Actor { Id = 2, Name = "Actor 2", IsActive = true }
        };

        var actorDtos = new List<ActorDto>
        {
            new ActorDto { Id = 1, Name = "Actor 1", IsActive = true },
            new ActorDto { Id = 2, Name = "Actor 2", IsActive = true }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(actors);

        _mockMapper.Setup(m => m.Map<IEnumerable<ActorDto>>(actors))
            .Returns(actorDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Actor 1");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnActor_WhenActorExists()
    {
        // Arrange
        var actor = new Actor { Id = 1, Name = "Test Actor", IsActive = true };
        var actorDto = new ActorDto { Id = 1, Name = "Test Actor", IsActive = true };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(actor);

        _mockMapper.Setup(m => m.Map<ActorDto>(actor))
            .Returns(actorDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Actor");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenActorDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Actor?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateActor_WhenValidDataProvided()
    {
        // Arrange
        var createDto = new ActorCreateDto
        {
            Name = "New Actor",
            Description = "New Description"
        };

        var createdActor = new Actor
        {
            Id = 1,
            Name = createDto.Name,
            Description = createDto.Description,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = "System"
        };

        var resultDto = new ActorDto
        {
            Id = 1,
            Name = createdActor.Name,
            Description = createdActor.Description,
            IsActive = true,
            CreatedDate = createdActor.CreatedDate,
            CreatedBy = "System"
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Actor>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdActor);

        _mockMapper.Setup(m => m.Map<ActorDto>(createdActor))
            .Returns(resultDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("New Actor");
        result.Description.Should().Be("New Description");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Actor>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenActorDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await FluentActions.Invoking(() => _service.DeleteAsync(999))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Actor with id 999 not found");
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingActors_WhenSearchTermProvided()
    {
        // Arrange
        var searchTerm = "test";
        var actors = new List<Actor>
        {
            new Actor { Id = 1, Name = "Test Actor", IsActive = true }
        };

        var actorDtos = new List<ActorDto>
        {
            new ActorDto { Id = 1, Name = "Test Actor", IsActive = true }
        };

        _mockRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
            .ReturnsAsync(actors);

        _mockMapper.Setup(m => m.Map<IEnumerable<ActorDto>>(actors))
            .Returns(actorDtos);

        // Act
        var result = await _service.SearchAsync(searchTerm);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Contain("Test");
    }
}