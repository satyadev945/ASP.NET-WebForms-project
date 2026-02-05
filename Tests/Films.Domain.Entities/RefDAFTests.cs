using Films.Domain.Entities;
using Xunit;

namespace Films.Domain.Entities.Tests
{
    public class RefDAFTests
    {
        [Fact]
        public void RefDAF_Constructor_CreatesInstance()
        {
            // Arrange & Act
            var refDAF = new RefDAF();

            // Assert
            Assert.NotNull(refDAF);
            Assert.Equal(0, refDAF.Id);
            Assert.Equal(0, refDAF.DirectedById);
            Assert.Equal(0, refDAF.FilmId);
        }

        [Fact]
        public void RefDAF_SetProperties_PropertiesAreSet()
        {
            // Arrange
            var refDAF = new RefDAF();
            var director = new DirectedBy { Id = 1, Name = "Christopher Nolan" };
            var film = new Film { Id = 2, Name = "Inception" };

            // Act
            refDAF.Id = 3;
            refDAF.DirectedById = 1;
            refDAF.FilmId = 2;
            refDAF.DirectedBy = director;
            refDAF.Film = film;

            // Assert
            Assert.Equal(3, refDAF.Id);
            Assert.Equal(1, refDAF.DirectedById);
            Assert.Equal(2, refDAF.FilmId);
            Assert.Same(director, refDAF.DirectedBy);
            Assert.Same(film, refDAF.Film);
        }

        [Fact]
        public void RefDAF_NavigationProperties_ReferencesMatchIds()
        {
            // Arrange
            var director = new DirectedBy { Id = 5 };
            var film = new Film { Id = 10 };

            // Act
            var refDAF = new RefDAF
            {
                DirectedById = 5,
                FilmId = 10,
                DirectedBy = director,
                Film = film
            };

            // Assert
            Assert.Equal(refDAF.DirectedById, refDAF.DirectedBy.Id);
            Assert.Equal(refDAF.FilmId, refDAF.Film.Id);
        }
    }
}