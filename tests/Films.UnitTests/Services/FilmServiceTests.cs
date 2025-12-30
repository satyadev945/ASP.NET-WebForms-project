using AutoMapper;
using Films.Application.Services;
using Films.Domain.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Films.UnitTests.Services;

/// <summary>
/// Unit tests for FilmService
/// </summary>
public class FilmServiceTests
{
    private readonly Mock<IFilmRepository> _mockFilmRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<FilmService>> _mockLogger;
    private readonly FilmService _filmService;

    public FilmServiceTests()
    {
        _mockFilmRepository = new Mock<IFilmRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<FilmService>>();
        _filmService = new FilmService(_mockFilmRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllFilms_WhenCalled()
    {
        // Arrange
        var films = new List<Film>
        {
            new Film { Id = 1, Name = "Test Film 1", Year = 2023 },
            new Film { Id = 2, Name = "Test Film 2", Year = 2024 }
        };
        var filmDtos = new List<FilmDto>
        {
            new FilmDto { Id = 1, Name = "Test Film 1", Year = 2023 },
            new FilmDto { Id = 2, Name = "Test Film 2", Year = 2024 }
        };

        _mockFilmRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(films);
        _mockMapper.Setup(m => m.Map<IEnumerable<FilmDto>>(films))
            .Returns(filmDtos);

        // Act
        var result = await _filmService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(filmDtos);
        _mockFilmRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFilm_WhenFilmExists()
    {
        // Arrange
        var filmId = 1;
        var film = new Film { Id = filmId, Name = "Test Film", Year = 2023 };
        var filmDto = new FilmDto { Id = filmId, Name = "Test Film", Year = 2023 };

        _mockFilmRepository.Setup(r => r.GetByIdAsync(filmId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(film);
        _mockMapper.Setup(m => m.Map<FilmDto>(film))
            .Returns(filmDto);

        // Act
        var result = await _filmService.GetByIdAsync(filmId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(filmDto);
        _mockFilmRepository.Verify(r => r.GetByIdAsync(filmId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenFilmDoesNotExist()
    {
        // Arrange
        var filmId = 999;

        _mockFilmRepository.Setup(r => r.GetByIdAsync(filmId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Film?)null);

        // Act
        var result = await _filmService.GetByIdAsync(filmId);

        // Assert
        result.Should().BeNull();
        _mockFilmRepository.Verify(r => r.GetByIdAsync(filmId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateAndReturnFilm_WhenValidDto()
    {
        // Arrange
        var createDto = new FilmCreateDto
        {
            Name = "New Film",
            Year = 2024,
            Genre = "Action",
            CreatedBy = "TestUser"
        };
        var film = new Film { Id = 0, Name = "New Film", Year = 2024, Genre = "Action" };
        var createdFilm = new Film { Id = 1, Name = "New Film", Year = 2024, Genre = "Action" };
        var filmDto = new FilmDto { Id = 1, Name = "New Film", Year = 2024, Genre = "Action" };

        _mockMapper.Setup(m => m.Map<Film>(createDto))
            .Returns(film);
        _mockFilmRepository.Setup(r => r.AddAsync(It.IsAny<Film>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdFilm);
        _mockMapper.Setup(m => m.Map<FilmDto>(createdFilm))
            .Returns(filmDto);

        // Act
        var result = await _filmService.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(filmDto);
        _mockFilmRepository.Verify(r => r.AddAsync(It.IsAny<Film>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAndReturnFilm_WhenFilmExists()
    {
        // Arrange
        var filmId = 1;
        var updateDto = new FilmUpdateDto
        {
            Name = "Updated Film",
            Year = 2024,
            ModifiedBy = "TestUser"
        };
        var existingFilm = new Film { Id = filmId, Name = "Old Film", Year = 2023 };
        var updatedFilm = new Film { Id = filmId, Name = "Updated Film", Year = 2024 };
        var filmDto = new FilmDto { Id = filmId, Name = "Updated Film", Year = 2024 };

        _mockFilmRepository.Setup(r => r.GetByIdAsync(filmId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingFilm);
        _mockMapper.Setup(m => m.Map(updateDto, existingFilm))
            .Returns(updatedFilm);
        _mockFilmRepository.Setup(r => r.UpdateAsync(It.IsAny<Film>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedFilm);
        _mockMapper.Setup(m => m.Map<FilmDto>(updatedFilm))
            .Returns(filmDto);

        // Act
        var result = await _filmService.UpdateAsync(filmId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(filmDto);
        _mockFilmRepository.Verify(r => r.GetByIdAsync(filmId, It.IsAny<CancellationToken>()), Times.Once);
        _mockFilmRepository.Verify(r => r.UpdateAsync(It.IsAny<Film>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenFilmDoesNotExist()
    {
        // Arrange
        var filmId = 999;
        var updateDto = new FilmUpdateDto { Name = "Updated Film", Year = 2024, ModifiedBy = "TestUser" };

        _mockFilmRepository.Setup(r => r.GetByIdAsync(filmId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Film?)null);

        // Act & Assert
        await _filmService.Invoking(s => s.UpdateAsync(filmId, updateDto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage($"Film with id {filmId} not found*");

        _mockFilmRepository.Verify(r => r.GetByIdAsync(filmId, It.IsAny<CancellationToken>()), Times.Once);
        _mockFilmRepository.Verify(r => r.UpdateAsync(It.IsAny<Film>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteFilm_WhenFilmExists()
    {
        // Arrange
        var filmId = 1;

        _mockFilmRepository.Setup(r => r.ExistsAsync(filmId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockFilmRepository.Setup(r => r.DeleteAsync(filmId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _filmService.DeleteAsync(filmId);

        // Assert
        _mockFilmRepository.Verify(r => r.ExistsAsync(filmId, It.IsAny<CancellationToken>()), Times.Once);
        _mockFilmRepository.Verify(r => r.DeleteAsync(filmId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenFilmDoesNotExist()
    {
        // Arrange
        var filmId = 999;

        _mockFilmRepository.Setup(r => r.ExistsAsync(filmId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await _filmService.Invoking(s => s.DeleteAsync(filmId))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage($"Film with id {filmId} not found*");

        _mockFilmRepository.Verify(r => r.ExistsAsync(filmId, It.IsAny<CancellationToken>()), Times.Once);
        _mockFilmRepository.Verify(r => r.DeleteAsync(filmId, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingFilms_WhenSearchTermProvided()
    {
        // Arrange
        var searchTerm = "Action";
        var films = new List<Film>
        {
            new Film { Id = 1, Name = "Action Film 1", Genre = "Action" },
            new Film { Id = 2, Name = "Action Film 2", Genre = "Action" }
        };
        var filmDtos = new List<FilmDto>
        {
            new FilmDto { Id = 1, Name = "Action Film 1", Genre = "Action" },
            new FilmDto { Id = 2, Name = "Action Film 2", Genre = "Action" }
        };

        _mockFilmRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
            .ReturnsAsync(films);
        _mockMapper.Setup(m => m.Map<IEnumerable<FilmDto>>(films))
            .Returns(filmDtos);

        // Act
        var result = await _filmService.SearchAsync(searchTerm);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(filmDtos);
        _mockFilmRepository.Verify(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()), Times.Once);
    }
}