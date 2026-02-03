using Films.Domain.Entities;

namespace Films.Application.Interfaces;

public interface IActorService
{
    Task<IEnumerable<Actor>> GetAllActorsAsync();
    Task<Actor?> GetActorByIdAsync(int id);
    Task<Actor> CreateActorAsync(Actor actor);
    Task<Actor> UpdateActorAsync(Actor actor);
    Task<bool> DeleteActorAsync(int id);
}