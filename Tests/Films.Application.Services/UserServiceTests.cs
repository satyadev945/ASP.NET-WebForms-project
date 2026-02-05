using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Films.Application.Common.Interfaces;
using Films.Application.Services;
using Films.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Xunit;

namespace Films.Application.Services.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IFilmsDbContext> _mockContext;
        private readonly Mock<DbSet<User>> _mockUserDbSet;
        private readonly Mock<DbSet<UserRight>> _mockUserRightDbSet;

        public UserServiceTests()
        {
            _mockContext = new Mock<IFilmsDbContext>();
            _mockUserDbSet = CreateMockDbSet<User>(new List<User>());
            _mockUserRightDbSet = CreateMockDbSet<UserRight>(new List<UserRight>());
            _mockContext.Setup(c => c.Users).Returns(_mockUserDbSet.Object);
            _mockContext.Setup(c => c.UserRights).Returns(_mockUserRightDbSet.Object);
        }

        private static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

            mockDbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(data.Add);

            return mockDbSet;
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Username = "admin", Email = "admin@example.com" },
                new User { Id = 2, Username = "user", Email = "user@example.com" }
            };

            var mockUserDbSet = CreateMockDbSet(users);
            mockUserDbSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockUserDbSet.Object);
            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);

            var service = new UserService(_mockContext.Object);

            // Act
            var result = await service.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, u => u.Id == 1 && u.Username == "admin" && u.Email == "admin@example.com");
            Assert.Contains(result, u => u.Id == 2 && u.Username == "user" && u.Email == "user@example.com");
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserExists_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "admin", Email = "admin@example.com" };
            var users = new List<User> { user };

            var mockUserDbSet = CreateMockDbSet(users);
            mockUserDbSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockUserDbSet.Object);
            mockUserDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<User, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<User, bool> predicate, CancellationToken token) =>
                    users.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);

            var service = new UserService(_mockContext.Object);

            // Act
            var result = await service.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("admin", result.Username);
            Assert.Equal("admin@example.com", result.Email);
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var users = new List<User>();

            var mockUserDbSet = CreateMockDbSet(users);
            mockUserDbSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockUserDbSet.Object);
            mockUserDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<User, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<User, bool> predicate, CancellationToken token) =>
                    users.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);

            var service = new UserService(_mockContext.Object);

            // Act
            var result = await service.GetUserByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_WhenUserExists_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "admin", Email = "admin@example.com" };
            var users = new List<User> { user };

            var mockUserDbSet = CreateMockDbSet(users);
            mockUserDbSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockUserDbSet.Object);
            mockUserDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<User, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<User, bool> predicate, CancellationToken token) =>
                    users.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);

            var service = new UserService(_mockContext.Object);

            // Act
            var result = await service.GetUserByUsernameAsync("admin");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("admin", result.Username);
            Assert.Equal("admin@example.com", result.Email);
        }

        [Fact]
        public async Task ValidateUserCredentialsAsync_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            var password = "password123";
            var hashedPassword = HashPassword(password);
            var user = new User { Id = 1, Username = "admin", Password = hashedPassword };
            var users = new List<User> { user };

            var mockUserDbSet = CreateMockDbSet(users);
            mockUserDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<User, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<User, bool> predicate, CancellationToken token) =>
                    users.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);

            var service = new UserService(_mockContext.Object);

            // Act
            var result = await service.ValidateUserCredentialsAsync("admin", password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateUserCredentialsAsync_WithInvalidPassword_ReturnsFalse()
        {
            // Arrange
            var user = new User { Id = 1, Username = "admin", Password = HashPassword("correctPassword") };
            var users = new List<User> { user };

            var mockUserDbSet = CreateMockDbSet(users);
            mockUserDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<User, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<User, bool> predicate, CancellationToken token) =>
                    users.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);

            var service = new UserService(_mockContext.Object);

            // Act
            var result = await service.ValidateUserCredentialsAsync("admin", "wrongPassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateUserCredentialsAsync_WithNonexistentUsername_ReturnsFalse()
        {
            // Arrange
            var users = new List<User>();

            var mockUserDbSet = CreateMockDbSet(users);
            mockUserDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<User, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<User, bool> predicate, CancellationToken token) =>
                    users.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);

            var service = new UserService(_mockContext.Object);

            // Act
            var result = await service.ValidateUserCredentialsAsync("nonexistent", "anyPassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateUserAsync_AddsUserToContext_AndReturnsUser()
        {
            // Arrange
            var users = new List<User>();
            var mockUserDbSet = CreateMockDbSet(users);

            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new UserService(_mockContext.Object);
            var user = new User { Username = "newuser", Email = "newuser@example.com" };
            var password = "password123";

            // Act
            var result = await service.CreateUserAsync(user, password);

            // Assert
            Assert.Same(user, result);
            Assert.Contains(user, users);
            Assert.NotEqual(password, user.Password); // Password should be hashed
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUser_AndReturnsUpdatedUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "admin", Email = "admin@example.com" };
            var mockEntry = new Mock<EntityEntry<User>>();
            mockEntry.Setup(e => e.State).Returns(EntityState.Modified);

            _mockContext.Setup(c => c.Entry(It.IsAny<User>())).Returns(mockEntry.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new UserService(_mockContext.Object);

            // Act
            user.Email = "updated@example.com";
            var result = await service.UpdateUserAsync(user);

            // Assert
            Assert.Same(user, result);
            _mockContext.Verify(c => c.Entry(user), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_WhenUserExists_RemovesUserAndReturnsTrue()
        {
            // Arrange
            var user = new User { Id = 1, Username = "admin", Email = "admin@example.com" };
            var users = new List<User> { user };
            var mockUserDbSet = CreateMockDbSet(users);

            mockUserDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => users.FirstOrDefault(u => u.Id == (int)ids[0]));

            mockUserDbSet.Setup(m => m.Remove(It.IsAny<User>()))
                .Callback<User>(u => users.Remove(u));

            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new UserService(_mockContext.Object);

            // Act
            var result = await service.DeleteUserAsync(1);

            // Assert
            Assert.True(result);
            Assert.Empty(users);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_WhenUserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var users = new List<User>();
            var mockUserDbSet = CreateMockDbSet(users);

            mockUserDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => users.FirstOrDefault(u => u.Id == (int)ids[0]));

            _mockContext.Setup(c => c.Users).Returns(mockUserDbSet.Object);

            var service = new UserService(_mockContext.Object);

            // Act
            var result = await service.DeleteUserAsync(1);

            // Assert
            Assert.False(result);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetUserRightsAsync_ReturnsUserRights()
        {
            // Arrange
            var right1 = new Right { Id = 1, Name = "ManageUsers" };
            var right2 = new Right { Id = 2, Name = "ManageFilms" };

            var userRights = new List<UserRight>
            {
                new UserRight { Id = 1, UserId = 1, RightId = 1, Right = right1 },
                new UserRight { Id = 2, UserId = 1, RightId = 2, Right = right2 }
            };

            var mockUserRightDbSet = CreateMockDbSet(userRights);
            mockUserRightDbSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockUserRightDbSet.Object);

            _mockContext.Setup(c => c.UserRights).Returns(mockUserRightDbSet.Object);

            var service = new UserService(_mockContext.Object);

            // Act
            var result = await service.GetUserRightsAsync(1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.Id == 1 && r.Name == "ManageUsers");
            Assert.Contains(result, r => r.Id == 2 && r.Name == "ManageFilms");
        }

        [Fact]
        public async Task AddRightToUserAsync_AddsUserRight()
        {
            // Arrange
            var userRights = new List<UserRight>();
            var mockUserRightDbSet = CreateMockDbSet(userRights);

            _mockContext.Setup(c => c.UserRights).Returns(mockUserRightDbSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new UserService(_mockContext.Object);

            // Act
            await service.AddRightToUserAsync(1, 2);

            // Assert
            Assert.Single(userRights);
            Assert.Equal(1, userRights[0].UserId);
            Assert.Equal(2, userRights[0].RightId);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RemoveRightFromUserAsync_WhenUserRightExists_RemovesUserRight()
        {
            // Arrange
            var userRight = new UserRight { Id = 1, UserId = 1, RightId = 2 };
            var userRights = new List<UserRight> { userRight };
            var mockUserRightDbSet = CreateMockDbSet(userRights);

            mockUserRightDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<UserRight, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<UserRight, bool> predicate, CancellationToken token) =>
                    userRights.FirstOrDefault(predicate));

            mockUserRightDbSet.Setup(m => m.Remove(It.IsAny<UserRight>()))
                .Callback<UserRight>(ur => userRights.Remove(ur));

            _mockContext.Setup(c => c.UserRights).Returns(mockUserRightDbSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new UserService(_mockContext.Object);

            // Act
            await service.RemoveRightFromUserAsync(1, 2);

            // Assert
            Assert.Empty(userRights);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RemoveRightFromUserAsync_WhenUserRightDoesNotExist_DoesNothing()
        {
            // Arrange
            var userRights = new List<UserRight>();
            var mockUserRightDbSet = CreateMockDbSet(userRights);

            mockUserRightDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Func<UserRight, bool>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Func<UserRight, bool> predicate, CancellationToken token) =>
                    userRights.FirstOrDefault(predicate));

            _mockContext.Setup(c => c.UserRights).Returns(mockUserRightDbSet.Object);

            var service = new UserService(_mockContext.Object);

            // Act
            await service.RemoveRightFromUserAsync(1, 2);

            // Assert
            Assert.Empty(userRights);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}