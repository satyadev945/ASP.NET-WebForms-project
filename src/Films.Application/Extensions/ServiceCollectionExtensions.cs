using Microsoft.Extensions.DependencyInjection;
using Films.Application.Interfaces;
using Films.Application.Mappings;
using Films.Application.Services;

namespace Films.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<IFilmService, FilmService>();
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IDirectorService, DirectorService>();

        return services;
    }
}
