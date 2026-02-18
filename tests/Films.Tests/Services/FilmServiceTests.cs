using Films.Application.Services;
using Films.Domain.Entities;
using Films.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Films.Tests.Services;

/// <summary>
/// Unit tests for FilmService
/// </summary>
public class FilmServiceTests
{
    private readonly Mock<IFilmRepository> _mockRepository;
    private readonly Mock<ILogger<FilmService>> _mockLogger;
    private readonly FilmService _service;

    public FilmServiceTests()
    {
        _mockRepository = new Mock<IFilmRepository>();
        _mockLogger = new Mock<ILogger<FilmService>>();
        _service = new FilmService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllFilmsAsync_ReturnsAllFilms()
    {
        // Arrange
        var films = new List<Film>
        {
            new Film { Id = 1, Title = "Film 1", Year = 2020 },
            new Film { Id = 2, Title = "Film 2", Year = 2021 }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(films);

        // Act
        var result = await _service.GetAllFilmsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetFilmByIdAsync_ReturnsFilm_WhenFilmExists()
    {
        // Arrange
        var film = new Film { Id = 1, Title = "Test Film", Year = 2020 };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(film);

        // Act
        var result = await _service.GetFilmByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Film", result.Title);
        _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreateFilmAsync_CreatesFilm_WithCreatedDate()
    {
        // Arrange
        var film = new Film { Title = "New Film", Year = 2023 };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Film>())).ReturnsAsync(film);

        // Act
        var result = await _service.CreateFilmAsync(film);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(DateTime.MinValue, film.CreatedDate);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Film>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFilmAsync_UpdatesFilm_WithModifiedDate()
    {
        // Arrange
        var film = new Film { Id = 1, Title = "Updated Film", Year = 2023 };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Film>())).ReturnsAsync(film);

        // Act
        var result = await _service.UpdateFilmAsync(film);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(film.ModifiedDate);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Film>()), Times.Once);
    }

    [Fact]
    public async Task DeleteFilmAsync_DeletesFilm_ReturnsTrue()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteFilmAsync(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task SearchFilmsAsync_ReturnsMatchingFilms()
    {
        // Arrange
        var films = new List<Film>
        {
            new Film { Id = 1, Title = "Action Film", Year = 2020 }
        };
        _mockRepository.Setup(r => r.SearchAsync("Action")).ReturnsAsync(films);

        // Act
        var result = await _service.SearchFilmsAsync("Action");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _mockRepository.Verify(r => r.SearchAsync("Action"), Times.Once);
    }
}
