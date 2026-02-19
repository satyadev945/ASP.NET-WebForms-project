using Xunit;
using Moq;
using Films.Web.Pages;
using Microsoft.Extensions.Logging;

namespace Films.Web.Tests;

public class IndexModelTests
{
    [Fact]
    public void Constructor_WithValidLogger_ShouldCreateInstance()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<IndexModel>>();

        // Act
        var model = new IndexModel(mockLogger.Object);

        // Assert
        Assert.NotNull(model);
    }

    [Fact]
    public void OnGet_ShouldLogInformation()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<IndexModel>>();
        var model = new IndexModel(mockLogger.Object);

        // Act
        model.OnGet();

        // Assert
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
