using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Actor entity
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
            return await _context.Actors
                .Where(a => a.IsActive)
                .Include(a => a.Sex)
                .AsNoTracking()
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
            return await _context.Actors
                .Where(a => a.Id == id && a.IsActive)
                .Include(a => a.Sex)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving actor with ID {ActorId} from database", id);
            throw;
        }
    }

    public async Task<Actor> AddAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Actors.AddAsync(actor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return actor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding actor to database");
            throw;
        }
    }

    public async Task UpdateAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Actors.Update(actor);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating actor in database");
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var actor = await _context.Actors.FindAsync(new object[] { id }, cancellationToken);
            if (actor != null)
            {
                actor.IsActive = false;
                actor.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting actor with ID {ActorId} from database", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Actors.AnyAsync(a => a.Id == id && a.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if actor with ID {ActorId} exists", id);
            throw;
        }
    }

    public async Task<IEnumerable<Actor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Actors
                .Where(a => a.IsActive &&
                    (a.Name.Contains(searchTerm) ||
                     a.Surname != null && a.Surname.Contains(searchTerm) ||
                     a.Bio != null && a.Bio.Contains(searchTerm)))
                .Include(a => a.Sex)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching actors with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
