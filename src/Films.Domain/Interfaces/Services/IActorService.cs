using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Services;

public interface IActorService
{
    Task<IEnumerable<Actor>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Actor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Actor> CreateAsync(string name, string? description = null, CancellationToken cancellationToken = default);
    Task<Actor> UpdateAsync(int id, string name, string? description = null, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Actor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}