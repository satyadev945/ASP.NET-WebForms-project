using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Films.IntegrationTests;

public class PagesTests : IClassFixture<WebApplicationFactory<Films.Web.Program>>
{
    private readonly WebApplicationFactory<Films.Web.Program> _factory;

    public PagesTests(WebApplicationFactory<Films.Web.Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task HomePage_ReturnsSuccess()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();
    }
}
