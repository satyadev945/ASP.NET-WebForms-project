using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Domain.Entities.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_Constructor_CreatesInstance()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.NotNull(user);
            Assert.Equal(0, user.Id);
            Assert.Equal(string.Empty, user.Username);
            Assert.Equal(string.Empty, user.Password);
            Assert.Equal(string.Empty, user.Email);
            Assert.Null(user.TypeUserId);
            Assert.Null(user.TypeUser);
            Assert.NotNull(user.UserRights);
            Assert.Empty(user.UserRights);
        }

        [Fact]
        public void User_SetProperties_PropertiesAreSet()
        {
            // Arrange
            var user = new User();
            var typeUser = new TypeUser { Id = 1, Name = "Administrator" };

            // Act
            user.Id = 1;
            user.Username = "admin";
            user.Password = "securePassword123";
            user.Email = "admin@example.com";
            user.TypeUserId = 1;
            user.TypeUser = typeUser;

            // Assert
            Assert.Equal(1, user.Id);
            Assert.Equal("admin", user.Username);
            Assert.Equal("securePassword123", user.Password);
            Assert.Equal("admin@example.com", user.Email);
            Assert.Equal(1, user.TypeUserId);
            Assert.Same(typeUser, user.TypeUser);
        }

        [Fact]
        public void User_AddUserRight_UserRightIsAdded()
        {
            // Arrange
            var user = new User { Id = 1 };
            var right = new Right { Id = 1, Name = "ManageUsers" };
            var userRight = new UserRight { UserId = 1, RightId = 1, Right = right };

            // Act
            user.UserRights.Add(userRight);

            // Assert
            Assert.Single(user.UserRights);
            Assert.Contains(userRight, user.UserRights);
        }
    }
}