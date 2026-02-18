using Films.Domain.Entities;

namespace Films.Application.Interfaces;

/// <summary>
/// Service interface for film operations
/// </summary>
public interface IFilmService
{
    Task<IEnumerable<Film>> GetAllFilmsAsync();
    Task<Film?> GetFilmByIdAsync(int id);
    Task<Film> CreateFilmAsync(Film film);
    Task<Film> UpdateFilmAsync(Film film);
    Task<bool> DeleteFilmAsync(int id);
    Task<IEnumerable<Film>> SearchFilmsAsync(string searchTerm);
}
