using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for film operations
/// </summary>
public class FilmService : IFilmService
{
    private readonly IFilmRepository _filmRepository;
    private readonly ILogger<FilmService> _logger;

    public FilmService(IFilmRepository filmRepository, ILogger<FilmService> logger)
    {
        _filmRepository = filmRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Film>> GetAllFilmsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all films");
            return await _filmRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all films");
            throw;
        }
    }

    public async Task<Film?> GetFilmByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving film with ID: {FilmId}", id);
            return await _filmRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<Film> CreateFilmAsync(Film film)
    {
        try
        {
            _logger.LogInformation("Creating new film: {FilmTitle}", film.Title);
            film.CreatedDate = DateTime.UtcNow;
            return await _filmRepository.AddAsync(film);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film: {FilmTitle}", film.Title);
            throw;
        }
    }

    public async Task<Film> UpdateFilmAsync(Film film)
    {
        try
        {
            _logger.LogInformation("Updating film with ID: {FilmId}", film.Id);
            film.ModifiedDate = DateTime.UtcNow;
            return await _filmRepository.UpdateAsync(film);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film with ID: {FilmId}", film.Id);
            throw;
        }
    }

    public async Task<bool> DeleteFilmAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting film with ID: {FilmId}", id);
            return await _filmRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Film>> SearchFilmsAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation("Searching films with term: {SearchTerm}", searchTerm);
            return await _filmRepository.SearchAsync(searchTerm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
