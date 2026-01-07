using Films.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Films.Application.Extensions;

/// <summary>
/// Extension methods for registering application services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
