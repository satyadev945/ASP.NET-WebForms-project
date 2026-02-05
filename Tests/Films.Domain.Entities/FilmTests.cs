using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Domain.Entities.Tests
{
    public class FilmTests
    {
        [Fact]
        public void Film_Constructor_CreatesInstance()
        {
            // Arrange & Act
            var film = new Film();

            // Assert
            Assert.NotNull(film);
            Assert.Equal(0, film.Id);
            Assert.Equal(string.Empty, film.Name);
            Assert.Null(film.Year);
            Assert.Null(film.Description);
            Assert.NotNull(film.ActorReferences);
            Assert.Empty(film.ActorReferences);
            Assert.NotNull(film.DirectorReferences);
            Assert.Empty(film.DirectorReferences);
        }

        [Fact]
        public void Film_SetProperties_PropertiesAreSet()
        {
            // Arrange
            var film = new Film();

            // Act
            film.Id = 1;
            film.Name = "Inception";
            film.Year = 2010;
            film.Description = "A thief who steals corporate secrets through the use of dream-sharing technology.";

            // Assert
            Assert.Equal(1, film.Id);
            Assert.Equal("Inception", film.Name);
            Assert.Equal(2010, film.Year);
            Assert.Equal("A thief who steals corporate secrets through the use of dream-sharing technology.", film.Description);
        }

        [Fact]
        public void Film_AddActorReference_ActorReferenceIsAdded()
        {
            // Arrange
            var film = new Film { Id = 1 };
            var actorRef = new RefAF { ActorId = 1, FilmId = 1 };

            // Act
            film.ActorReferences.Add(actorRef);

            // Assert
            Assert.Single(film.ActorReferences);
            Assert.Contains(actorRef, film.ActorReferences);
        }

        [Fact]
        public void Film_AddDirectorReference_DirectorReferenceIsAdded()
        {
            // Arrange
            var film = new Film { Id = 1 };
            var directorRef = new RefDAF { DirectedById = 1, FilmId = 1 };

            // Act
            film.DirectorReferences.Add(directorRef);

            // Assert
            Assert.Single(film.DirectorReferences);
            Assert.Contains(directorRef, film.DirectorReferences);
        }
    }
}