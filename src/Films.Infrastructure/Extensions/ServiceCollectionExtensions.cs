using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Films.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Films.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<FilmsDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(FilmsDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IFilmRepository, FilmRepository>();
        services.AddScoped<IDirectedByRepository, DirectedByRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}