using Films.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Films.IntegrationTests.Fixtures;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real database context
            services.RemoveAll(typeof(DbContextOptions<FilmsDbContext>));

            // Add in-memory database for testing
            services.AddDbContext<FilmsDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database context
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<FilmsDbContext>();

            // Ensure the database is created
            db.Database.EnsureCreated();

            // Seed the database with test data
            SeedTestData(db);
        });

        builder.UseEnvironment("Testing");
    }

    private static void SeedTestData(FilmsDbContext context)
    {
        // Clear existing data
        context.Actors.RemoveRange(context.Actors);
        context.Films.RemoveRange(context.Films);
        context.SaveChanges();

        // Seed test data
        var testActor = new Films.Domain.Entities.Actor
        {
            Name = "Test Actor",
            Description = "Test Description",
            CreatedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "Test"
        };

        var testFilm = new Films.Domain.Entities.Film
        {
            Name = "Test Film",
            Description = "Test Film Description",
            Genre = "Action",
            Year = 2023,
            Director = "Test Director",
            Country = "USA",
            Duration = 120,
            Rating = 8.5m,
            CreatedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "Test"
        };

        context.Actors.Add(testActor);
        context.Films.Add(testFilm);
        context.SaveChanges();
    }
}