using Xunit;

namespace Films.Web.Tests;

/// <summary>
/// Unit tests for Program.cs startup configuration
/// Note: These are placeholder tests since Program class is not directly testable
/// Integration tests should be created separately for testing the full application startup
/// </summary>
public class ProgramTests
{
    [Fact]
    public void Program_ShouldHaveMainEntryPoint()
    {
        // Arrange & Act
        var programType = typeof(Films.Web.Controllers.HomeController).Assembly.GetTypes()
            .FirstOrDefault(t => t.Name == "Program");

        // Assert
        Assert.NotNull(programType);
    }

    [Fact]
    public void Program_AssemblyShouldBeLoaded()
    {
        // Arrange & Act
        var assembly = typeof(Films.Web.Controllers.HomeController).Assembly;

        // Assert
        Assert.NotNull(assembly);
        Assert.Contains("Films.Web", assembly.FullName);
    }

    [Fact]
    public void Program_ShouldHaveControllersInAssembly()
    {
        // Arrange
        var assembly = typeof(Films.Web.Controllers.HomeController).Assembly;

        // Act
        var controllerTypes = assembly.GetTypes()
            .Where(t => t.Name.EndsWith("Controller"))
            .ToList();

        // Assert
        Assert.NotEmpty(controllerTypes);
        Assert.Contains(controllerTypes, t => t.Name == "HomeController");
        Assert.Contains(controllerTypes, t => t.Name == "ActorsController");
        Assert.Contains(controllerTypes, t => t.Name == "FilmsController");
    }

    [Fact]
    public void Program_ShouldHaveModelsInAssembly()
    {
        // Arrange
        var assembly = typeof(Films.Web.Controllers.HomeController).Assembly;

        // Act
        var modelTypes = assembly.GetTypes()
            .Where(t => t.Namespace != null && t.Namespace.Contains("Models"))
            .ToList();

        // Assert
        Assert.NotEmpty(modelTypes);
    }

    [Fact]
    public void Program_AssemblyShouldTargetNet80()
    {
        // Arrange
        var assembly = typeof(Films.Web.Controllers.HomeController).Assembly;

        // Act
        var targetFramework = assembly.GetCustomAttributes(typeof(System.Runtime.Versioning.TargetFrameworkAttribute), false)
            .FirstOrDefault() as System.Runtime.Versioning.TargetFrameworkAttribute;

        // Assert
        Assert.NotNull(targetFramework);
        Assert.Contains(".NETCoreApp,Version=v8.0", targetFramework.FrameworkName);
    }

    [Fact]
    public void Program_ShouldHaveErrorViewModelInModels()
    {
        // Arrange
        var assembly = typeof(Films.Web.Controllers.HomeController).Assembly;

        // Act
        var errorViewModel = assembly.GetTypes()
            .FirstOrDefault(t => t.Name == "ErrorViewModel");

        // Assert
        Assert.NotNull(errorViewModel);
    }

    [Fact]
    public void Program_ShouldHaveThreeControllers()
    {
        // Arrange
        var assembly = typeof(Films.Web.Controllers.HomeController).Assembly;

        // Act
        var controllerTypes = assembly.GetTypes()
            .Where(t => t.Name.EndsWith("Controller") && 
                       t.Namespace != null && 
                       t.Namespace.Contains("Controllers"))
            .ToList();

        // Assert
        Assert.True(controllerTypes.Count >= 3, $"Expected at least 3 controllers, found {controllerTypes.Count}");
    }

    [Fact]
    public void Program_AssemblyShouldHaveCorrectName()
    {
        // Arrange & Act
        var assembly = typeof(Films.Web.Controllers.HomeController).Assembly;
        var assemblyName = assembly.GetName().Name;

        // Assert
        Assert.Equal("Films.Web", assemblyName);
    }

    [Fact]
    public void Program_AssemblyShouldHaveVersion()
    {
        // Arrange & Act
        var assembly = typeof(Films.Web.Controllers.HomeController).Assembly;
        var version = assembly.GetName().Version;

        // Assert
        Assert.NotNull(version);
    }

    [Fact]
    public void Program_ShouldHaveControllersNamespace()
    {
        // Arrange
        var assembly = typeof(Films.Web.Controllers.HomeController).Assembly;

        // Act
        var hasControllersNamespace = assembly.GetTypes()
            .Any(t => t.Namespace != null && t.Namespace == "Films.Web.Controllers");

        // Assert
        Assert.True(hasControllersNamespace);
    }
}
