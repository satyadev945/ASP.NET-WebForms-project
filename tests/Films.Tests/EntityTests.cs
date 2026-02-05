using Films.Domain.Entities;
using System;
using Xunit;

namespace Films.Tests
{
    public class EntityTests
    {
        [Fact]
        public void Actor_PropertiesWork()
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

            // Assert
            Assert.Equal(1, actor.Id);
            Assert.Equal("John", actor.FirstName);
            Assert.Equal("Doe", actor.LastName);
            Assert.Equal(dateOfBirth, actor.DateOfBirth);
            Assert.Equal(1, actor.SexId);
        }

        [Fact]
        public void Film_PropertiesWork()
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
        public void DirectedBy_PropertiesWork()
        {
            // Arrange
            var director = new DirectedBy();

            // Act
            director.Id = 1;
            director.Name = "Christopher Nolan";

            // Assert
            Assert.Equal(1, director.Id);
            Assert.Equal("Christopher Nolan", director.Name);
        }

        [Fact]
        public void User_PropertiesWork()
        {
            // Arrange
            var user = new User();

            // Act
            user.Id = 1;
            user.Username = "admin";
            user.Password = "securePassword123";
            user.Email = "admin@example.com";
            user.TypeUserId = 1;

            // Assert
            Assert.Equal(1, user.Id);
            Assert.Equal("admin", user.Username);
            Assert.Equal("securePassword123", user.Password);
            Assert.Equal("admin@example.com", user.Email);
            Assert.Equal(1, user.TypeUserId);
        }

        [Fact]
        public void Sex_PropertiesWork()
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
        public void Right_PropertiesWork()
        {
            // Arrange
            var right = new Right();

            // Act
            right.Id = 1;
            right.Name = "ManageUsers";

            // Assert
            Assert.Equal(1, right.Id);
            Assert.Equal("ManageUsers", right.Name);
        }

        [Fact]
        public void TypeUser_PropertiesWork()
        {
            // Arrange
            var typeUser = new TypeUser();

            // Act
            typeUser.Id = 1;
            typeUser.Name = "Administrator";

            // Assert
            Assert.Equal(1, typeUser.Id);
            Assert.Equal("Administrator", typeUser.Name);
        }
    }
}