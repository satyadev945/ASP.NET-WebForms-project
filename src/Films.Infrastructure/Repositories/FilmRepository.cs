using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

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
                .Where(x => x.IsActive)
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all films from database");
            throw;
        }
    }

    public async Task<Film?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting film with id {Id} from database", id);
            return await _context.Films
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting film with id {Id} from database", id);
            throw;
        }
    }

    public async Task<Film> AddAsync(Film film, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding new film to database: {Name}", film.Name);
            _context.Films.Add(film);
            await _context.SaveChangesAsync(cancellationToken);
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
            _logger.LogDebug("Soft deleting film with id {Id}", id);
            var film = await _context.Films
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (film != null)
            {
                film.IsActive = false;
                film.ModifiedDate = DateTime.UtcNow;
                film.ModifiedBy = "System";
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking if film exists with id {Id}", id);
            return await _context.Films
                .AsNoTracking()
                .AnyAsync(x => x.Id == id && x.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if film exists with id {Id}", id);
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
                .Where(x => x.IsActive &&
                           (x.Name.Contains(searchTerm) ||
                            (x.Description != null && x.Description.Contains(searchTerm)) ||
                            (x.Genre != null && x.Genre.Contains(searchTerm)) ||
                            (x.Director != null && x.Director.Contains(searchTerm))))
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}