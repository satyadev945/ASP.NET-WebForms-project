using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Domain.Entities.Tests
{
    public class DirectedByTests
    {
        [Fact]
        public void DirectedBy_Constructor_CreatesInstance()
        {
            // Arrange & Act
            var directedBy = new DirectedBy();

            // Assert
            Assert.NotNull(directedBy);
            Assert.Equal(0, directedBy.Id);
            Assert.Equal(string.Empty, directedBy.Name);
            Assert.NotNull(directedBy.FilmReferences);
            Assert.Empty(directedBy.FilmReferences);
        }

        [Fact]
        public void DirectedBy_SetProperties_PropertiesAreSet()
        {
            // Arrange
            var directedBy = new DirectedBy();

            // Act
            directedBy.Id = 1;
            directedBy.Name = "Steven Spielberg";

            // Assert
            Assert.Equal(1, directedBy.Id);
            Assert.Equal("Steven Spielberg", directedBy.Name);
        }

        [Fact]
        public void DirectedBy_AddFilmReference_FilmReferenceIsAdded()
        {
            // Arrange
            var directedBy = new DirectedBy { Id = 1 };
            var filmRef = new RefDAF { DirectedById = 1, FilmId = 1 };

            // Act
            directedBy.FilmReferences.Add(filmRef);

            // Assert
            Assert.Single(directedBy.FilmReferences);
            Assert.Contains(filmRef, directedBy.FilmReferences);
        }
    }
}