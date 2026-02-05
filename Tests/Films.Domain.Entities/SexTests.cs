using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Domain.Entities.Tests
{
    public class SexTests
    {
        [Fact]
        public void Sex_Constructor_CreatesInstance()
        {
            // Arrange & Act
            var sex = new Sex();

            // Assert
            Assert.NotNull(sex);
            Assert.Equal(0, sex.Id);
            Assert.Equal(string.Empty, sex.Name);
            Assert.NotNull(sex.Actors);
            Assert.Empty(sex.Actors);
        }

        [Fact]
        public void Sex_SetProperties_PropertiesAreSet()
        {
            // Arrange
            var sex = new Sex();

            // Act
            sex.Id = 1;
            sex.Name = "Male";

            // Assert
            Assert.Equal(1, sex.Id);
            Assert.Equal("Male", sex.Name);
        }

        [Fact]
        public void Sex_AddActor_ActorIsAdded()
        {
            // Arrange
            var sex = new Sex { Id = 1, Name = "Female" };
            var actor = new Actor { Id = 1, FirstName = "Meryl", LastName = "Streep", SexId = 1 };

            // Act
            sex.Actors.Add(actor);

            // Assert
            Assert.Single(sex.Actors);
            Assert.Contains(actor, sex.Actors);
        }
    }
}