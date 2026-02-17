using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Services;

/// <summary>
/// Service implementation for film operations
/// </summary>
public class FilmService : IFilmService
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<FilmService> _logger;

    public FilmService(FilmsDbContext context, ILogger<FilmService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Film>> GetAllFilmsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all films");
            return await _context.Films
                .Include(f => f.ActorFilms)
                .Include(f => f.DirectorFilms)
                .ToListAsync();
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
            return await _context.Films
                .Include(f => f.ActorFilms)
                .Include(f => f.DirectorFilms)
                .FirstOrDefaultAsync(f => f.Id == id);
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
            _context.Films.Add(film);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Film created successfully with ID: {FilmId}", film.Id);
            return film;
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
            _context.Films.Update(film);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Film updated successfully with ID: {FilmId}", film.Id);
            return film;
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
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                _logger.LogWarning("Film with ID: {FilmId} not found", id);
                return false;
            }

            _context.Films.Remove(film);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Film deleted successfully with ID: {FilmId}", id);
            return true;
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
            return await _context.Films
                .Where(f => f.Title.Contains(searchTerm) || 
                           (f.Description != null && f.Description.Contains(searchTerm)))
                .Include(f => f.ActorFilms)
                .Include(f => f.DirectorFilms)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
