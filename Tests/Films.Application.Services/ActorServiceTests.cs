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
    public class ActorServiceTests
    {
        private readonly Mock<IFilmsDbContext> _mockContext;
        private readonly Mock<DbSet<Actor>> _mockActorDbSet;

        public ActorServiceTests()
        {
            _mockContext = new Mock<IFilmsDbContext>();
            _mockActorDbSet = CreateMockDbSet<Actor>(new List<Actor>());
            _mockContext.Setup(c => c.Actors).Returns(_mockActorDbSet.Object);
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
        public async Task GetAllActorsAsync_ReturnsAllActors()
        {
            // Arrange
            var actors = new List<Actor>
            {
                new Actor { Id = 1, FirstName = "Tom", LastName = "Hanks" },
                new Actor { Id = 2, FirstName = "Meryl", LastName = "Streep" }
            };

            var mockActorDbSet = CreateMockDbSet(actors);
            mockActorDbSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockActorDbSet.Object);
            _mockContext.Setup(c => c.Actors).Returns(mockActorDbSet.Object);

            var service = new ActorService(_mockContext.Object);

            // Act
            var result = await service.GetAllActorsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, a => a.Id == 1 && a.FirstName == "Tom" && a.LastName == "Hanks");
            Assert.Contains(result, a => a.Id == 2 && a.FirstName == "Meryl" && a.LastName == "Streep");
        }

        [Fact]
        public async Task GetActorByIdAsync_WhenActorExists_ReturnsActor()
        {
            // Arrange
            var actor = new Actor { Id = 1, FirstName = "Tom", LastName = "Hanks" };
            var actors = new List<Actor> { actor };

            var mockActorDbSet = CreateMockDbSet(actors);
            mockActorDbSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockActorDbSet.Object);
            mockActorDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<Actor, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<Actor, bool> predicate, CancellationToken token) =>
                    actors.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.Actors).Returns(mockActorDbSet.Object);

            var service = new ActorService(_mockContext.Object);

            // Act
            var result = await service.GetActorByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Tom", result.FirstName);
            Assert.Equal("Hanks", result.LastName);
        }

        [Fact]
        public async Task GetActorByIdAsync_WhenActorDoesNotExist_ReturnsNull()
        {
            // Arrange
            var actors = new List<Actor>();

            var mockActorDbSet = CreateMockDbSet(actors);
            mockActorDbSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockActorDbSet.Object);
            mockActorDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<Actor, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<Actor, bool> predicate, CancellationToken token) =>
                    actors.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.Actors).Returns(mockActorDbSet.Object);

            var service = new ActorService(_mockContext.Object);

            // Act
            var result = await service.GetActorByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateActorAsync_AddsActorToContext_AndReturnsActor()
        {
            // Arrange
            var actors = new List<Actor>();
            var mockActorDbSet = CreateMockDbSet(actors);

            _mockContext.Setup(c => c.Actors).Returns(mockActorDbSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new ActorService(_mockContext.Object);
            var actor = new Actor { FirstName = "Tom", LastName = "Hanks" };

            // Act
            var result = await service.CreateActorAsync(actor);

            // Assert
            Assert.Same(actor, result);
            Assert.Contains(actor, actors);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateActorAsync_UpdatesActor_AndReturnsUpdatedActor()
        {
            // Arrange
            var actor = new Actor { Id = 1, FirstName = "Tom", LastName = "Hanks" };
            var mockEntry = new Mock<EntityEntry<Actor>>();
            mockEntry.Setup(e => e.State).Returns(EntityState.Modified);

            _mockContext.Setup(c => c.Entry(It.IsAny<Actor>())).Returns(mockEntry.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new ActorService(_mockContext.Object);

            // Act
            actor.FirstName = "Thomas";
            var result = await service.UpdateActorAsync(actor);

            // Assert
            Assert.Same(actor, result);
            _mockContext.Verify(c => c.Entry(actor), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteActorAsync_WhenActorExists_RemovesActorAndReturnsTrue()
        {
            // Arrange
            var actor = new Actor { Id = 1, FirstName = "Tom", LastName = "Hanks" };
            var actors = new List<Actor> { actor };
            var mockActorDbSet = CreateMockDbSet(actors);

            mockActorDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => actors.FirstOrDefault(a => a.Id == (int)ids[0]));

            mockActorDbSet.Setup(m => m.Remove(It.IsAny<Actor>()))
                .Callback<Actor>(a => actors.Remove(a));

            _mockContext.Setup(c => c.Actors).Returns(mockActorDbSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new ActorService(_mockContext.Object);

            // Act
            var result = await service.DeleteActorAsync(1);

            // Assert
            Assert.True(result);
            Assert.Empty(actors);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteActorAsync_WhenActorDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var actors = new List<Actor>();
            var mockActorDbSet = CreateMockDbSet(actors);

            mockActorDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => actors.FirstOrDefault(a => a.Id == (int)ids[0]));

            _mockContext.Setup(c => c.Actors).Returns(mockActorDbSet.Object);

            var service = new ActorService(_mockContext.Object);

            // Act
            var result = await service.DeleteActorAsync(1);

            // Assert
            Assert.False(result);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}