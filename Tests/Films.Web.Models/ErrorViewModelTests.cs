using Films.Web.Models;
using Xunit;

namespace Films.Web.Models.Tests
{
    public class ErrorViewModelTests
    {
        [Fact]
        public void ShowRequestId_WhenRequestIdIsNull_ReturnsFalse()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel { RequestId = null };

            // Act & Assert
            Assert.False(errorViewModel.ShowRequestId);
        }

        [Fact]
        public void ShowRequestId_WhenRequestIdIsEmpty_ReturnsFalse()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel { RequestId = string.Empty };

            // Act & Assert
            Assert.False(errorViewModel.ShowRequestId);
        }

        [Fact]
        public void ShowRequestId_WhenRequestIdHasValue_ReturnsTrue()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel { RequestId = "123" };

            // Act & Assert
            Assert.True(errorViewModel.ShowRequestId);
        }

        [Fact]
        public void RequestId_CanBeSetAndGet()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel();
            var requestId = "test-request-id";

            // Act
            errorViewModel.RequestId = requestId;

            // Assert
            Assert.Equal(requestId, errorViewModel.RequestId);
        }
    }
}