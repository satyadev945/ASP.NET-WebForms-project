using Films.IntegrationTests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace Films.IntegrationTests.Pages;

public class ActorsPageTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ActorsPageTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_ActorsIndex_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/Actors");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.ToString().Should().Contain("text/html");
    }

    [Fact]
    public async Task Get_ActorsIndex_ContainsExpectedContent()
    {
        // Act
        var response = await _client.GetAsync("/Actors");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        content.Should().Contain("Actors");
        content.Should().Contain("Add New Actor");
    }

    [Fact]
    public async Task Get_ActorsCreate_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/Actors/Create");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.ToString().Should().Contain("text/html");
    }

    [Fact]
    public async Task Get_ActorsCreate_ContainsForm()
    {
        // Act
        var response = await _client.GetAsync("/Actors/Create");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        content.Should().Contain("Create Actor");
        content.Should().Contain("form");
        content.Should().Contain("Name");
        content.Should().Contain("Description");
    }
}