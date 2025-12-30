using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Film repository implementation using Entity Framework Core
/// </summary>
public class FilmRepository : IFilmRepository
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<FilmRepository> _logger;

    public FilmRepository(FilmsDbContext context, ILogger<FilmRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Film>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting all films from database");
            return await _context.Films
                .AsNoTracking()
                .OrderBy(f => f.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all films from database");
            throw;
        }
    }

    public async Task<Film?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting film by id: {Id}", id);
            return await _context.Films
                .AsNoTracking()
                .Include(f => f.RefAFs)
                    .ThenInclude(r => r.Actor)
                .Include(f => f.RefDAFs)
                    .ThenInclude(r => r.Director)
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving film by id: {Id}", id);
            throw;
        }
    }

    public async Task<Film> AddAsync(Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding film to database: {Name}", film.Name);
            _context.Films.Add(film);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("Film added successfully with id: {Id}", film.Id);
            return film;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding film to database: {Name}", film.Name);
            throw;
        }
    }

    public async Task<Film> UpdateAsync(Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating film in database: {Id}", film.Id);
            _context.Films.Update(film);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("Film updated successfully: {Id}", film.Id);
            return film;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film in database: {Id}", film.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting film from database: {Id}", id);
            var film = await _context.Films.FindAsync(new object[] { id }, cancellationToken);
            if (film != null)
            {
                // Soft delete
                film.IsActive = false;
                film.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogDebug("Film soft deleted successfully: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film from database: {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking if film exists: {Id}", id);
            return await _context.Films
                .AsNoTracking()
                .AnyAsync(f => f.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if film exists: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Film>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Searching films with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync(cancellationToken);

            return await _context.Films
                .AsNoTracking()
                .Where(f => f.Name.Contains(searchTerm) ||
                           (f.Description != null && f.Description.Contains(searchTerm)) ||
                           (f.Genre != null && f.Genre.Contains(searchTerm)))
                .OrderBy(f => f.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<Film>> GetByActorAsync(int actorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting films by actor: {ActorId}", actorId);
            return await _context.Films
                .AsNoTracking()
                .Where(f => f.RefAFs.Any(r => r.ActorId == actorId))
                .OrderBy(f => f.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting films by actor: {ActorId}", actorId);
            throw;
        }
    }

    public async Task<IEnumerable<Film>> GetByDirectorAsync(int directorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting films by director: {DirectorId}", directorId);
            return await _context.Films
                .AsNoTracking()
                .Where(f => f.RefDAFs.Any(r => r.DirectorId == directorId))
                .OrderBy(f => f.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting films by director: {DirectorId}", directorId);
            throw;
        }
    }

    public async Task<IEnumerable<Film>> GetByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting films by year: {Year}", year);
            return await _context.Films
                .AsNoTracking()
                .Where(f => f.Year == year)
                .OrderBy(f => f.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting films by year: {Year}", year);
            throw;
        }
    }

    public async Task<IEnumerable<Film>> GetByGenreAsync(string genre, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting films by genre: {Genre}", genre);
            return await _context.Films
                .AsNoTracking()
                .Where(f => f.Genre != null && f.Genre.Contains(genre))
                .OrderBy(f => f.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting films by genre: {Genre}", genre);
            throw;
        }
    }
}