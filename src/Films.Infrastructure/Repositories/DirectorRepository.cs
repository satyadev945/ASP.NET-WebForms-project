using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Director repository implementation using Entity Framework Core
/// </summary>
public class DirectorRepository : IDirectorRepository
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<DirectorRepository> _logger;

    public DirectorRepository(FilmsDbContext context, ILogger<DirectorRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Director>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting all directors from database");
            return await _context.Directors
                .AsNoTracking()
                .Include(d => d.Sex)
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all directors from database");
            throw;
        }
    }

    public async Task<Director?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting director by id: {Id}", id);
            return await _context.Directors
                .AsNoTracking()
                .Include(d => d.Sex)
                .Include(d => d.RefDAFs)
                    .ThenInclude(r => r.Film)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving director by id: {Id}", id);
            throw;
        }
    }

    public async Task<Director> AddAsync(Director director, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding director to database: {Name}", director.Name);
            _context.Directors.Add(director);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("Director added successfully with id: {Id}", director.Id);
            return director;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding director to database: {Name}", director.Name);
            throw;
        }
    }

    public async Task<Director> UpdateAsync(Director director, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating director in database: {Id}", director.Id);
            _context.Directors.Update(director);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("Director updated successfully: {Id}", director.Id);
            return director;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director in database: {Id}", director.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting director from database: {Id}", id);
            var director = await _context.Directors.FindAsync(new object[] { id }, cancellationToken);
            if (director != null)
            {
                // Soft delete
                director.IsActive = false;
                director.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogDebug("Director soft deleted successfully: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting director from database: {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking if director exists: {Id}", id);
            return await _context.Directors
                .AsNoTracking()
                .AnyAsync(d => d.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if director exists: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Director>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Searching directors with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync(cancellationToken);

            return await _context.Directors
                .AsNoTracking()
                .Include(d => d.Sex)
                .Where(d => d.Name.Contains(searchTerm) ||
                           (d.Description != null && d.Description.Contains(searchTerm)))
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching directors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<Director>> GetByFilmAsync(int filmId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting directors by film: {FilmId}", filmId);
            return await _context.Directors
                .AsNoTracking()
                .Include(d => d.Sex)
                .Where(d => d.RefDAFs.Any(r => r.FilmId == filmId))
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting directors by film: {FilmId}", filmId);
            throw;
        }
    }

    public async Task<IEnumerable<Director>> GetBySexAsync(int sexId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting directors by sex: {SexId}", sexId);
            return await _context.Directors
                .AsNoTracking()
                .Include(d => d.Sex)
                .Where(d => d.SexId == sexId)
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting directors by sex: {SexId}", sexId);
            throw;
        }
    }
}