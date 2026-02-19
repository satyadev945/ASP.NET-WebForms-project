using Xunit;
using Films.Web.Pages;

namespace Films.Web.Tests;

public class AboutModelTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance()
    {
        // Arrange & Act
        var model = new AboutModel();

        // Assert
        Assert.NotNull(model);
    }

    [Fact]
    public void OnGet_ShouldExecuteWithoutException()
    {
        // Arrange
        var model = new AboutModel();

        // Act
        var exception = Record.Exception(() => model.OnGet());

        // Assert
        Assert.Null(exception);
    }
}
