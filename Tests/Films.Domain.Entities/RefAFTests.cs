using Films.Domain.Entities;
using Xunit;

namespace Films.Domain.Entities.Tests
{
    public class RefAFTests
    {
        [Fact]
        public void RefAF_Constructor_CreatesInstance()
        {
            // Arrange & Act
            var refAF = new RefAF();

            // Assert
            Assert.NotNull(refAF);
            Assert.Equal(0, refAF.Id);
            Assert.Equal(0, refAF.ActorId);
            Assert.Equal(0, refAF.FilmId);
        }

        [Fact]
        public void RefAF_SetProperties_PropertiesAreSet()
        {
            // Arrange
            var refAF = new RefAF();
            var actor = new Actor { Id = 1, FirstName = "Brad", LastName = "Pitt" };
            var film = new Film { Id = 2, Name = "Fight Club" };

            // Act
            refAF.Id = 3;
            refAF.ActorId = 1;
            refAF.FilmId = 2;
            refAF.Actor = actor;
            refAF.Film = film;

            // Assert
            Assert.Equal(3, refAF.Id);
            Assert.Equal(1, refAF.ActorId);
            Assert.Equal(2, refAF.FilmId);
            Assert.Same(actor, refAF.Actor);
            Assert.Same(film, refAF.Film);
        }

        [Fact]
        public void RefAF_NavigationProperties_ReferencesMatchIds()
        {
            // Arrange
            var actor = new Actor { Id = 5 };
            var film = new Film { Id = 10 };

            // Act
            var refAF = new RefAF
            {
                ActorId = 5,
                FilmId = 10,
                Actor = actor,
                Film = film
            };

            // Assert
            Assert.Equal(refAF.ActorId, refAF.Actor.Id);
            Assert.Equal(refAF.FilmId, refAF.Film.Id);
        }
    }
}