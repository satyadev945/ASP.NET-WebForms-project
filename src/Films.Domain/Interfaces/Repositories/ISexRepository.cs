using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Sex entity
/// </summary>
public interface ISexRepository
{
    Task<IEnumerable<Sex>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Sex?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Sex> AddAsync(Sex sex, CancellationToken cancellationToken = default);
    Task<Sex> UpdateAsync(Sex sex, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sex>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}