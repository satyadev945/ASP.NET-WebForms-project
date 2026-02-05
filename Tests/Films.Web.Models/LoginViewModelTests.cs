using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Films.Web.Models;
using Xunit;

namespace Films.Web.Models.Tests
{
    public class LoginViewModelTests
    {
        [Fact]
        public void LoginViewModel_HasRequiredAttributes()
        {
            // Arrange
            var model = new LoginViewModel();
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            // Act & Assert
            // Validation should fail because required fields are not set
            Assert.False(Validator.TryValidateObject(model, context, results, true));
            Assert.Equal(2, results.Count); // Two required fields
        }

        [Fact]
        public void Username_HasRequiredAttribute()
        {
            // Arrange
            var property = typeof(LoginViewModel).GetProperty("Username");

            // Act
            var requiredAttribute = property.GetCustomAttributes(typeof(RequiredAttribute), false);
            var displayAttribute = property.GetCustomAttributes(typeof(DisplayAttribute), false);

            // Assert
            Assert.NotEmpty(requiredAttribute);
            Assert.NotEmpty(displayAttribute);
            Assert.Equal("Username", ((DisplayAttribute)displayAttribute[0]).Name);
        }

        [Fact]
        public void Password_HasRequiredAndDataTypeAttribute()
        {
            // Arrange
            var property = typeof(LoginViewModel).GetProperty("Password");

            // Act
            var requiredAttribute = property.GetCustomAttributes(typeof(RequiredAttribute), false);
            var dataTypeAttribute = property.GetCustomAttributes(typeof(DataTypeAttribute), false);
            var displayAttribute = property.GetCustomAttributes(typeof(DisplayAttribute), false);

            // Assert
            Assert.NotEmpty(requiredAttribute);
            Assert.NotEmpty(dataTypeAttribute);
            Assert.NotEmpty(displayAttribute);
            Assert.Equal(DataType.Password, ((DataTypeAttribute)dataTypeAttribute[0]).DataType);
            Assert.Equal("Password", ((DisplayAttribute)displayAttribute[0]).Name);
        }

        [Fact]
        public void RememberMe_HasDisplayAttribute()
        {
            // Arrange
            var property = typeof(LoginViewModel).GetProperty("RememberMe");

            // Act
            var displayAttribute = property.GetCustomAttributes(typeof(DisplayAttribute), false);

            // Assert
            Assert.NotEmpty(displayAttribute);
            Assert.Equal("Remember me?", ((DisplayAttribute)displayAttribute[0]).Name);
        }

        [Fact]
        public void LoginViewModel_DefaultValues()
        {
            // Arrange & Act
            var model = new LoginViewModel();

            // Assert
            Assert.Equal(string.Empty, model.Username);
            Assert.Equal(string.Empty, model.Password);
            Assert.False(model.RememberMe);
        }

        [Fact]
        public void LoginViewModel_CanSetAndGetProperties()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Username = "testuser",
                Password = "password123",
                RememberMe = true
            };

            // Act & Assert
            Assert.Equal("testuser", model.Username);
            Assert.Equal("password123", model.Password);
            Assert.True(model.RememberMe);
        }
    }
}