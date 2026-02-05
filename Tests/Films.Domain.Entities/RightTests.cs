using System.Collections.Generic;
using Films.Domain.Entities;
using Xunit;

namespace Films.Domain.Entities.Tests
{
    public class RightTests
    {
        [Fact]
        public void Right_Constructor_CreatesInstance()
        {
            // Arrange & Act
            var right = new Right();

            // Assert
            Assert.NotNull(right);
            Assert.Equal(0, right.Id);
            Assert.Equal(string.Empty, right.Name);
            Assert.NotNull(right.UserRights);
            Assert.Empty(right.UserRights);
        }

        [Fact]
        public void Right_SetProperties_PropertiesAreSet()
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
        public void Right_AddUserRight_UserRightIsAdded()
        {
            // Arrange
            var right = new Right { Id = 1, Name = "ManageContent" };
            var user = new User { Id = 1, Username = "admin" };
            var userRight = new UserRight { UserId = 1, RightId = 1, User = user };

            // Act
            right.UserRights.Add(userRight);

            // Assert
            Assert.Single(right.UserRights);
            Assert.Contains(userRight, right.UserRights);
        }
    }
}