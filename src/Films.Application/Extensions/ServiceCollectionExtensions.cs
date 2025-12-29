using Films.Application.Interfaces;
using Films.Application.Mappings;
using Films.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Films.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<IFilmService, FilmService>();

        return services;
    }
}
