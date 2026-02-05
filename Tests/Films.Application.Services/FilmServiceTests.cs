using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Films.Application.Common.Interfaces;
using Films.Application.Services;
using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Xunit;

namespace Films.Application.Services.Tests
{
    public class FilmServiceTests
    {
        private readonly Mock<IFilmsDbContext> _mockContext;
        private readonly Mock<DbSet<Film>> _mockFilmDbSet;

        public FilmServiceTests()
        {
            _mockContext = new Mock<IFilmsDbContext>();
            _mockFilmDbSet = CreateMockDbSet<Film>(new List<Film>());
            _mockContext.Setup(c => c.Films).Returns(_mockFilmDbSet.Object);
        }

        private static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

            mockDbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(data.Add);

            return mockDbSet;
        }

        [Fact]
        public async Task GetAllFilmsAsync_ReturnsAllFilms()
        {
            // Arrange
            var films = new List<Film>
            {
                new Film { Id = 1, Name = "Inception", Year = 2010 },
                new Film { Id = 2, Name = "The Shawshank Redemption", Year = 1994 }
            };

            var mockFilmDbSet = CreateMockDbSet(films);
            _mockContext.Setup(c => c.Films).Returns(mockFilmDbSet.Object);

            var service = new FilmService(_mockContext.Object);

            // Act
            var result = await service.GetAllFilmsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, f => f.Id == 1 && f.Name == "Inception" && f.Year == 2010);
            Assert.Contains(result, f => f.Id == 2 && f.Name == "The Shawshank Redemption" && f.Year == 1994);
        }

        [Fact]
        public async Task GetFilmByIdAsync_WhenFilmExists_ReturnsFilm()
        {
            // Arrange
            var film = new Film { Id = 1, Name = "Inception", Year = 2010 };
            var films = new List<Film> { film };

            var mockFilmDbSet = CreateMockDbSet(films);
            mockFilmDbSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockFilmDbSet.Object);
            mockFilmDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<Film, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<Film, bool> predicate, CancellationToken token) =>
                    films.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.Films).Returns(mockFilmDbSet.Object);

            var service = new FilmService(_mockContext.Object);

            // Act
            var result = await service.GetFilmByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Inception", result.Name);
            Assert.Equal(2010, result.Year);
        }

        [Fact]
        public async Task GetFilmByIdAsync_WhenFilmDoesNotExist_ReturnsNull()
        {
            // Arrange
            var films = new List<Film>();

            var mockFilmDbSet = CreateMockDbSet(films);
            mockFilmDbSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockFilmDbSet.Object);
            mockFilmDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<Film, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<Film, bool> predicate, CancellationToken token) =>
                    films.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.Films).Returns(mockFilmDbSet.Object);

            var service = new FilmService(_mockContext.Object);

            // Act
            var result = await service.GetFilmByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateFilmAsync_AddsFilmToContext_AndReturnsFilm()
        {
            // Arrange
            var films = new List<Film>();
            var mockFilmDbSet = CreateMockDbSet(films);

            _mockContext.Setup(c => c.Films).Returns(mockFilmDbSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new FilmService(_mockContext.Object);
            var film = new Film { Name = "Inception", Year = 2010 };

            // Act
            var result = await service.CreateFilmAsync(film);

            // Assert
            Assert.Same(film, result);
            Assert.Contains(film, films);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateFilmAsync_UpdatesFilm_AndReturnsUpdatedFilm()
        {
            // Arrange
            var film = new Film { Id = 1, Name = "Inception", Year = 2010 };
            var mockEntry = new Mock<EntityEntry<Film>>();
            mockEntry.Setup(e => e.State).Returns(EntityState.Modified);

            _mockContext.Setup(c => c.Entry(It.IsAny<Film>())).Returns(mockEntry.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new FilmService(_mockContext.Object);

            // Act
            film.Name = "Inception (Director's Cut)";
            var result = await service.UpdateFilmAsync(film);

            // Assert
            Assert.Same(film, result);
            _mockContext.Verify(c => c.Entry(film), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteFilmAsync_WhenFilmExists_RemovesFilmAndReturnsTrue()
        {
            // Arrange
            var film = new Film { Id = 1, Name = "Inception", Year = 2010 };
            var films = new List<Film> { film };
            var mockFilmDbSet = CreateMockDbSet(films);

            mockFilmDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => films.FirstOrDefault(f => f.Id == (int)ids[0]));

            mockFilmDbSet.Setup(m => m.Remove(It.IsAny<Film>()))
                .Callback<Film>(f => films.Remove(f));

            _mockContext.Setup(c => c.Films).Returns(mockFilmDbSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new FilmService(_mockContext.Object);

            // Act
            var result = await service.DeleteFilmAsync(1);

            // Assert
            Assert.True(result);
            Assert.Empty(films);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteFilmAsync_WhenFilmDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var films = new List<Film>();
            var mockFilmDbSet = CreateMockDbSet(films);

            mockFilmDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => films.FirstOrDefault(f => f.Id == (int)ids[0]));

            _mockContext.Setup(c => c.Films).Returns(mockFilmDbSet.Object);

            var service = new FilmService(_mockContext.Object);

            // Act
            var result = await service.DeleteFilmAsync(1);

            // Assert
            Assert.False(result);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}