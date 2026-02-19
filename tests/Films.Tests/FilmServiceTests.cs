using Films.Domain.Entities;
using Films.Infrastructure.Data;
using Films.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Films.Tests;

public class FilmServiceTests
{
    private readonly Mock<ILogger<FilmService>> _loggerMock;
    private readonly FilmsDbContext _context;
    private readonly FilmService _filmService;

    public FilmServiceTests()
    {
        _loggerMock = new Mock<ILogger<FilmService>>();
        
        var options = new DbContextOptionsBuilder<FilmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new FilmsDbContext(options);
        _filmService = new FilmService(_context, _loggerMock.Object);
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
        
        await _context.Films.AddRangeAsync(films);
        await _context.SaveChangesAsync();

        // Act
        var result = await _filmService.GetAllFilmsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetFilmByIdAsync_ReturnsFilm_WhenFilmExists()
    {
        // Arrange
        var film = new Film { Id = 1, Title = "Test Film", Year = 2020 };
        await _context.Films.AddAsync(film);
        await _context.SaveChangesAsync();

        // Act
        var result = await _filmService.GetFilmByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Film", result.Title);
    }

    [Fact]
    public async Task CreateFilmAsync_AddsFilm()
    {
        // Arrange
        var film = new Film { Title = "New Film", Year = 2023 };

        // Act
        var result = await _filmService.CreateFilmAsync(film);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("New Film", result.Title);
    }

    [Fact]
    public async Task DeleteFilmAsync_RemovesFilm_WhenFilmExists()
    {
        // Arrange
        var film = new Film { Id = 1, Title = "Film to Delete", Year = 2020 };
        await _context.Films.AddAsync(film);
        await _context.SaveChangesAsync();

        // Act
        var result = await _filmService.DeleteFilmAsync(1);

        // Assert
        Assert.True(result);
        var deletedFilm = await _context.Films.FindAsync(1);
        Assert.Null(deletedFilm);
    }

    [Fact]
    public async Task SearchFilmsAsync_ReturnsMatchingFilms()
    {
        // Arrange
        var films = new List<Film>
        {
            new Film { Id = 1, Title = "Action Movie", Year = 2020 },
            new Film { Id = 2, Title = "Comedy Film", Year = 2021 },
            new Film { Id = 3, Title = "Action Hero", Year = 2022 }
        };
        
        await _context.Films.AddRangeAsync(films);
        await _context.SaveChangesAsync();

        // Act
        var result = await _filmService.SearchFilmsAsync("Action");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}
