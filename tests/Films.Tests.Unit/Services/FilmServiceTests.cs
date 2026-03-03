using Films.Application.DTOs;
using Films.Application.Services;
using Films.Domain.Entities;
using Films.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;

namespace Films.Tests.Unit.Services;

public class FilmServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Film>> _mockFilmRepository;
    private readonly Mock<ILogger<FilmService>> _mockLogger;
    private readonly FilmService _filmService;

    public FilmServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockFilmRepository = new Mock<IRepository<Film>>();
        _mockLogger = new Mock<ILogger<FilmService>>();
        
        _mockUnitOfWork.Setup(u => u.Films).Returns(_mockFilmRepository.Object);
        
        _filmService = new FilmService(_mockUnitOfWork.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllFilmsAsync_ShouldReturnAllFilms()
    {
        // Arrange
        var films = new List<Film>
        {
            new Film { Id = 1, Title = "Film 1", Year = 2020 },
            new Film { Id = 2, Title = "Film 2", Year = 2021 }
        };
        _mockFilmRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(films);

        // Act
        var result = await _filmService.GetAllFilmsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(f => f.Title == "Film 1");
    }

    [Fact]
    public async Task GetFilmByIdAsync_WhenFilmExists_ShouldReturnFilm()
    {
        // Arrange
        var film = new Film { Id = 1, Title = "Test Film", Year = 2020 };
        _mockFilmRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(film);

        // Act
        var result = await _filmService.GetFilmByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Test Film");
    }

    [Fact]
    public async Task CreateFilmAsync_ShouldCreateFilm()
    {
        // Arrange
        var createDto = new CreateFilmDto { Title = "New Film", Year = 2023 };
        var film = new Film { Id = 1, Title = "New Film", Year = 2023 };
        _mockFilmRepository.Setup(r => r.AddAsync(It.IsAny<Film>())).ReturnsAsync(film);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _filmService.CreateFilmAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New Film");
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
