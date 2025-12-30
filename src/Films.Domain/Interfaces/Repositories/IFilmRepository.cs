using Films.Domain.Entities;

namespace Films.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Film entity
/// </summary>
public interface IFilmRepository
{
    Task<IEnumerable<Film>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Film?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Film> AddAsync(Film film, CancellationToken cancellationToken = default);
    Task<Film> UpdateAsync(Film film, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Film>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<Film>> GetByActorAsync(int actorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Film>> GetByDirectorAsync(int directorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Film>> GetByYearAsync(int year, CancellationToken cancellationToken = default);
    Task<IEnumerable<Film>> GetByGenreAsync(string genre, CancellationToken cancellationToken = default);
}