using Films.Application.Common.Interfaces;
using Films.Application.Interfaces;
using Films.Application.Services;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Films.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FilmsDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(FilmsDbContext).Assembly.FullName)));

        // Register DBContext interface
        services.AddScoped<IFilmsDbContext>(provider => provider.GetRequiredService<FilmsDbContext>());

        // Register services
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IDirectedByService, DirectedByService>();
        services.AddScoped<IFilmService, FilmService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}