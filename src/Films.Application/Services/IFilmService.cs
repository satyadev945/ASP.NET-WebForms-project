using Films.Application.DTOs;

namespace Films.Application.Services;

/// <summary>
/// Service interface for film operations
/// </summary>
public interface IFilmService
{
    Task<IEnumerable<FilmDto>> GetAllFilmsAsync();
    Task<FilmDto?> GetFilmByIdAsync(int id);
    Task<FilmDto> CreateFilmAsync(CreateFilmDto createFilmDto);
    Task UpdateFilmAsync(UpdateFilmDto updateFilmDto);
    Task DeleteFilmAsync(int id);
}
