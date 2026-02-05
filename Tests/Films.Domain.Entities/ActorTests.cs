using System;
using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Domain.Entities.Tests
{
    public class ActorTests
    {
        [Fact]
        public void Actor_Constructor_CreatesInstance()
        {
            // Arrange & Act
            var actor = new Actor();

            // Assert
            Assert.NotNull(actor);
            Assert.Equal(0, actor.Id);
            Assert.Equal(string.Empty, actor.FirstName);
            Assert.Equal(string.Empty, actor.LastName);
            Assert.Null(actor.DateOfBirth);
            Assert.Null(actor.SexId);
            Assert.Null(actor.Sex);
            Assert.NotNull(actor.FilmReferences);
            Assert.Empty(actor.FilmReferences);
        }

        [Fact]
        public void Actor_SetProperties_PropertiesAreSet()
        {
            // Arrange
            var actor = new Actor();
            var dateOfBirth = new DateTime(1980, 1, 1);

            // Act
            actor.Id = 1;
            actor.FirstName = "John";
            actor.LastName = "Doe";
            actor.DateOfBirth = dateOfBirth;
            actor.SexId = 1;
            actor.Sex = new Sex { Id = 1, Name = "Male" };

            // Assert
            Assert.Equal(1, actor.Id);
            Assert.Equal("John", actor.FirstName);
            Assert.Equal("Doe", actor.LastName);
            Assert.Equal(dateOfBirth, actor.DateOfBirth);
            Assert.Equal(1, actor.SexId);
            Assert.NotNull(actor.Sex);
            Assert.Equal(1, actor.Sex.Id);
        }

        [Fact]
        public void Actor_AddFilmReference_FilmReferenceIsAdded()
        {
            // Arrange
            var actor = new Actor { Id = 1 };
            var filmRef = new RefAF { ActorId = 1, FilmId = 1 };

            // Act
            actor.FilmReferences.Add(filmRef);

            // Assert
            Assert.Single(actor.FilmReferences);
            Assert.Contains(filmRef, actor.FilmReferences);
        }
    }
}