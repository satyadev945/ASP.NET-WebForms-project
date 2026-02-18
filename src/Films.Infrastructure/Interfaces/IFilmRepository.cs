using Films.Domain.Entities;

namespace Films.Infrastructure.Interfaces;

/// <summary>
/// Repository interface for film data access
/// </summary>
public interface IFilmRepository
{
    Task<IEnumerable<Film>> GetAllAsync();
    Task<Film?> GetByIdAsync(int id);
    Task<Film> AddAsync(Film film);
    Task<Film> UpdateAsync(Film film);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Film>> SearchAsync(string searchTerm);
}
