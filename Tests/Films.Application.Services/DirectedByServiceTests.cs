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
    public class DirectedByServiceTests
    {
        private readonly Mock<IFilmsDbContext> _mockContext;
        private readonly Mock<DbSet<DirectedBy>> _mockDirectorDbSet;

        public DirectedByServiceTests()
        {
            _mockContext = new Mock<IFilmsDbContext>();
            _mockDirectorDbSet = CreateMockDbSet<DirectedBy>(new List<DirectedBy>());
            _mockContext.Setup(c => c.DirectedBys).Returns(_mockDirectorDbSet.Object);
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
        public async Task GetAllDirectorsAsync_ReturnsAllDirectors()
        {
            // Arrange
            var directors = new List<DirectedBy>
            {
                new DirectedBy { Id = 1, Name = "Christopher Nolan" },
                new DirectedBy { Id = 2, Name = "Steven Spielberg" }
            };

            var mockDirectorDbSet = CreateMockDbSet(directors);
            _mockContext.Setup(c => c.DirectedBys).Returns(mockDirectorDbSet.Object);

            var service = new DirectedByService(_mockContext.Object);

            // Act
            var result = await service.GetAllDirectorsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, d => d.Id == 1 && d.Name == "Christopher Nolan");
            Assert.Contains(result, d => d.Id == 2 && d.Name == "Steven Spielberg");
        }

        [Fact]
        public async Task GetDirectorByIdAsync_WhenDirectorExists_ReturnsDirector()
        {
            // Arrange
            var director = new DirectedBy { Id = 1, Name = "Christopher Nolan" };
            var directors = new List<DirectedBy> { director };

            var mockDirectorDbSet = CreateMockDbSet(directors);
            mockDirectorDbSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockDirectorDbSet.Object);
            mockDirectorDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<DirectedBy, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<DirectedBy, bool> predicate, CancellationToken token) =>
                    directors.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.DirectedBys).Returns(mockDirectorDbSet.Object);

            var service = new DirectedByService(_mockContext.Object);

            // Act
            var result = await service.GetDirectorByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Christopher Nolan", result.Name);
        }

        [Fact]
        public async Task GetDirectorByIdAsync_WhenDirectorDoesNotExist_ReturnsNull()
        {
            // Arrange
            var directors = new List<DirectedBy>();

            var mockDirectorDbSet = CreateMockDbSet(directors);
            mockDirectorDbSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockDirectorDbSet.Object);
            mockDirectorDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<DirectedBy, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<DirectedBy, bool> predicate, CancellationToken token) =>
                    directors.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.DirectedBys).Returns(mockDirectorDbSet.Object);

            var service = new DirectedByService(_mockContext.Object);

            // Act
            var result = await service.GetDirectorByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateDirectorAsync_AddsDirectorToContext_AndReturnsDirector()
        {
            // Arrange
            var directors = new List<DirectedBy>();
            var mockDirectorDbSet = CreateMockDbSet(directors);

            _mockContext.Setup(c => c.DirectedBys).Returns(mockDirectorDbSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new DirectedByService(_mockContext.Object);
            var director = new DirectedBy { Name = "Christopher Nolan" };

            // Act
            var result = await service.CreateDirectorAsync(director);

            // Assert
            Assert.Same(director, result);
            Assert.Contains(director, directors);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDirectorAsync_UpdatesDirector_AndReturnsUpdatedDirector()
        {
            // Arrange
            var director = new DirectedBy { Id = 1, Name = "Christopher Nolan" };
            var mockEntry = new Mock<EntityEntry<DirectedBy>>();
            mockEntry.Setup(e => e.State).Returns(EntityState.Modified);

            _mockContext.Setup(c => c.Entry(It.IsAny<DirectedBy>())).Returns(mockEntry.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new DirectedByService(_mockContext.Object);

            // Act
            director.Name = "Chris Nolan";
            var result = await service.UpdateDirectorAsync(director);

            // Assert
            Assert.Same(director, result);
            _mockContext.Verify(c => c.Entry(director), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDirectorAsync_WhenDirectorExists_RemovesDirectorAndReturnsTrue()
        {
            // Arrange
            var director = new DirectedBy { Id = 1, Name = "Christopher Nolan" };
            var directors = new List<DirectedBy> { director };
            var mockDirectorDbSet = CreateMockDbSet(directors);

            mockDirectorDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => directors.FirstOrDefault(d => d.Id == (int)ids[0]));

            mockDirectorDbSet.Setup(m => m.Remove(It.IsAny<DirectedBy>()))
                .Callback<DirectedBy>(d => directors.Remove(d));

            _mockContext.Setup(c => c.DirectedBys).Returns(mockDirectorDbSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new DirectedByService(_mockContext.Object);

            // Act
            var result = await service.DeleteDirectorAsync(1);

            // Assert
            Assert.True(result);
            Assert.Empty(directors);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDirectorAsync_WhenDirectorDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var directors = new List<DirectedBy>();
            var mockDirectorDbSet = CreateMockDbSet(directors);

            mockDirectorDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => directors.FirstOrDefault(d => d.Id == (int)ids[0]));

            _mockContext.Setup(c => c.DirectedBys).Returns(mockDirectorDbSet.Object);

            var service = new DirectedByService(_mockContext.Object);

            // Act
            var result = await service.DeleteDirectorAsync(1);

            // Assert
            Assert.False(result);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}