using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Actor repository implementation using Entity Framework Core
/// </summary>
public class ActorRepository : IActorRepository
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<ActorRepository> _logger;

    public ActorRepository(FilmsDbContext context, ILogger<ActorRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Actor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting all actors from database");
            return await _context.Actors
                .AsNoTracking()
                .Include(a => a.Sex)
                .OrderBy(a => a.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all actors from database");
            throw;
        }
    }

    public async Task<Actor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting actor by id: {Id}", id);
            return await _context.Actors
                .AsNoTracking()
                .Include(a => a.Sex)
                .Include(a => a.RefAFs)
                    .ThenInclude(r => r.Film)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving actor by id: {Id}", id);
            throw;
        }
    }

    public async Task<Actor> AddAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding actor to database: {Name}", actor.Name);
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("Actor added successfully with id: {Id}", actor.Id);
            return actor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding actor to database: {Name}", actor.Name);
            throw;
        }
    }

    public async Task<Actor> UpdateAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating actor in database: {Id}", actor.Id);
            _context.Actors.Update(actor);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("Actor updated successfully: {Id}", actor.Id);
            return actor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating actor in database: {Id}", actor.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting actor from database: {Id}", id);
            var actor = await _context.Actors.FindAsync(new object[] { id }, cancellationToken);
            if (actor != null)
            {
                // Soft delete
                actor.IsActive = false;
                actor.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogDebug("Actor soft deleted successfully: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting actor from database: {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking if actor exists: {Id}", id);
            return await _context.Actors
                .AsNoTracking()
                .AnyAsync(a => a.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if actor exists: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Actor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Searching actors with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync(cancellationToken);

            return await _context.Actors
                .AsNoTracking()
                .Include(a => a.Sex)
                .Where(a => a.Name.Contains(searchTerm) ||
                           (a.Description != null && a.Description.Contains(searchTerm)))
                .OrderBy(a => a.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching actors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<Actor>> GetByFilmAsync(int filmId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting actors by film: {FilmId}", filmId);
            return await _context.Actors
                .AsNoTracking()
                .Include(a => a.Sex)
                .Where(a => a.RefAFs.Any(r => r.FilmId == filmId))
                .OrderBy(a => a.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting actors by film: {FilmId}", filmId);
            throw;
        }
    }

    public async Task<IEnumerable<Actor>> GetBySexAsync(int sexId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting actors by sex: {SexId}", sexId);
            return await _context.Actors
                .AsNoTracking()
                .Include(a => a.Sex)
                .Where(a => a.SexId == sexId)
                .OrderBy(a => a.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting actors by sex: {SexId}", sexId);
            throw;
        }
    }
}