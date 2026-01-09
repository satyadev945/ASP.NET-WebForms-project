using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Director entity
/// </summary>
public interface IDirectorService
{
    Task<IEnumerable<DirectedBy>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DirectedBy?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DirectedBy> CreateAsync(DirectedBy director, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, DirectedBy director, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DirectedBy>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
