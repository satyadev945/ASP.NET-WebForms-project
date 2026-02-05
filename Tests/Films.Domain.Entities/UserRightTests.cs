using Films.Domain.Entities;
using Xunit;

namespace Films.Domain.Entities.Tests
{
    public class UserRightTests
    {
        [Fact]
        public void UserRight_Constructor_CreatesInstance()
        {
            // Arrange & Act
            var userRight = new UserRight();

            // Assert
            Assert.NotNull(userRight);
            Assert.Equal(0, userRight.Id);
            Assert.Equal(0, userRight.UserId);
            Assert.Equal(0, userRight.RightId);
        }

        [Fact]
        public void UserRight_SetProperties_PropertiesAreSet()
        {
            // Arrange
            var userRight = new UserRight();
            var user = new User { Id = 1, Username = "admin" };
            var right = new Right { Id = 2, Name = "ManageFilms" };

            // Act
            userRight.Id = 3;
            userRight.UserId = 1;
            userRight.RightId = 2;
            userRight.User = user;
            userRight.Right = right;

            // Assert
            Assert.Equal(3, userRight.Id);
            Assert.Equal(1, userRight.UserId);
            Assert.Equal(2, userRight.RightId);
            Assert.Same(user, userRight.User);
            Assert.Same(right, userRight.Right);
        }

        [Fact]
        public void UserRight_NavigationProperties_ReferencesMatchIds()
        {
            // Arrange
            var user = new User { Id = 5 };
            var right = new Right { Id = 10 };

            // Act
            var userRight = new UserRight
            {
                UserId = 5,
                RightId = 10,
                User = user,
                Right = right
            };

            // Assert
            Assert.Equal(userRight.UserId, userRight.User.Id);
            Assert.Equal(userRight.RightId, userRight.Right.Id);
        }
    }
}