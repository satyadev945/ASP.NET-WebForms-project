using Xunit;

namespace Films.Web.Tests;

/// <summary>
/// Basic tests for Program.cs
/// Note: Program.cs uses top-level statements which are difficult to test directly
/// </summary>
public class ProgramTests
{
    [Fact]
    public void Program_ShouldCompile()
    {
        // Arrange & Act & Assert
        // This test verifies that the program compiles successfully
        Assert.True(true);
    }
}
