using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Film entity
/// </summary>
public class FilmService : IFilmService
{
    private readonly IFilmRepository _filmRepository;
    private readonly ILogger<FilmService> _logger;

    public FilmService(IFilmRepository filmRepository, ILogger<FilmService> logger)
    {
        _filmRepository = filmRepository ?? throw new ArgumentNullException(nameof(filmRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Film>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching all films");
            return await _filmRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all films");
            throw;
        }
    }

    public async Task<Film?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching film with ID: {FilmId}", id);
            return await _filmRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<Film> CreateAsync(Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new film: {FilmName}", film.Name);
            film.CreatedDate = DateTime.UtcNow;
            film.IsActive = true;
            film.CreatedBy = "System";
            return await _filmRepository.AddAsync(film, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film: {FilmName}", film.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating film with ID: {FilmId}", id);
            var existing = await _filmRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Film with ID {id} not found");
            }

            film.Id = id;
            film.ModifiedDate = DateTime.UtcNow;
            film.ModifiedBy = "System";
            film.CreatedDate = existing.CreatedDate;
            film.CreatedBy = existing.CreatedBy;

            await _filmRepository.UpdateAsync(film, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting film with ID: {FilmId}", id);
            await _filmRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Film>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching films with term: {SearchTerm}", searchTerm);
            return await _filmRepository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
