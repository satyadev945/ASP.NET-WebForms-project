using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

public class DirectedByRepository : IDirectedByRepository
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<DirectedByRepository> _logger;

    public DirectedByRepository(FilmsDbContext context, ILogger<DirectedByRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<DirectedBy>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting all directors from database");
            return await _context.Directors
                .Where(x => x.IsActive)
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all directors from database");
            throw;
        }
    }

    public async Task<DirectedBy?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting director with id {Id} from database", id);
            return await _context.Directors
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting director with id {Id} from database", id);
            throw;
        }
    }

    public async Task<DirectedBy> AddAsync(DirectedBy directedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding new director to database: {Name}", directedBy.Name);
            _context.Directors.Add(directedBy);
            await _context.SaveChangesAsync(cancellationToken);
            return directedBy;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding director to database: {Name}", directedBy.Name);
            throw;
        }
    }

    public async Task<DirectedBy> UpdateAsync(DirectedBy directedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating director in database: {Id}", directedBy.Id);
            _context.Directors.Update(directedBy);
            await _context.SaveChangesAsync(cancellationToken);
            return directedBy;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director in database: {Id}", directedBy.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Soft deleting director with id {Id}", id);
            var director = await _context.Directors
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (director != null)
            {
                director.IsActive = false;
                director.ModifiedDate = DateTime.UtcNow;
                director.ModifiedBy = "System";
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting director with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking if director exists with id {Id}", id);
            return await _context.Directors
                .AsNoTracking()
                .AnyAsync(x => x.Id == id && x.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if director exists with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<DirectedBy>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Searching directors with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync(cancellationToken);

            return await _context.Directors
                .Where(x => x.IsActive &&
                           (x.Name.Contains(searchTerm) ||
                            (x.Description != null && x.Description.Contains(searchTerm))))
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching directors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}