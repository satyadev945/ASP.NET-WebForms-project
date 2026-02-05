using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Domain.Entities.Tests
{
    public class TypeUserTests
    {
        [Fact]
        public void TypeUser_Constructor_CreatesInstance()
        {
            // Arrange & Act
            var typeUser = new TypeUser();

            // Assert
            Assert.NotNull(typeUser);
            Assert.Equal(0, typeUser.Id);
            Assert.Equal(string.Empty, typeUser.Name);
            Assert.NotNull(typeUser.Users);
            Assert.Empty(typeUser.Users);
        }

        [Fact]
        public void TypeUser_SetProperties_PropertiesAreSet()
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

        [Fact]
        public void TypeUser_AddUser_UserIsAdded()
        {
            // Arrange
            var typeUser = new TypeUser { Id = 1, Name = "Standard User" };
            var user = new User { Id = 1, Username = "testuser", TypeUserId = 1 };

            // Act
            typeUser.Users.Add(user);

            // Assert
            Assert.Single(typeUser.Users);
            Assert.Contains(user, typeUser.Users);
        }
    }
}