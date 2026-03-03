using Films.Application.DTOs;

namespace Films.Application.Services;

/// <summary>
/// Service interface for actor operations
/// </summary>
public interface IActorService
{
    Task<IEnumerable<ActorDto>> GetAllActorsAsync();
    Task<ActorDto?> GetActorByIdAsync(int id);
    Task<ActorDto> CreateActorAsync(CreateActorDto createActorDto);
    Task UpdateActorAsync(UpdateActorDto updateActorDto);
    Task DeleteActorAsync(int id);
}
