using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Director entity
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

    public async Task<IEnumerable<DirectedBy>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Directors
                .Where(d => d.IsActive)
                .Include(d => d.Sex)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all directors from database");
            throw;
        }
    }

    public async Task<DirectedBy?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Directors
                .Where(d => d.Id == id && d.IsActive)
                .Include(d => d.Sex)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving director with ID {DirectorId} from database", id);
            throw;
        }
    }

    public async Task<DirectedBy> AddAsync(DirectedBy director, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Directors.AddAsync(director, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return director;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding director to database");
            throw;
        }
    }

    public async Task UpdateAsync(DirectedBy director, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Directors.Update(director);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director in database");
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var director = await _context.Directors.FindAsync(new object[] { id }, cancellationToken);
            if (director != null)
            {
                director.IsActive = false;
                director.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting director with ID {DirectorId} from database", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Directors.AnyAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if director with ID {DirectorId} exists", id);
            throw;
        }
    }

    public async Task<IEnumerable<DirectedBy>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Directors
                .Where(d => d.IsActive &&
                    (d.Name.Contains(searchTerm) ||
                     d.Surname != null && d.Surname.Contains(searchTerm) ||
                     d.Bio != null && d.Bio.Contains(searchTerm)))
                .Include(d => d.Sex)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching directors with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
