using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Repositories;

/// <summary>
/// Sex repository implementation using Entity Framework Core
/// </summary>
public class SexRepository : ISexRepository
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<SexRepository> _logger;

    public SexRepository(FilmsDbContext context, ILogger<SexRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Sex>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting all sexes from database");
            return await _context.Sexes
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all sexes from database");
            throw;
        }
    }

    public async Task<Sex?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting sex by id: {Id}", id);
            return await _context.Sexes
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sex by id: {Id}", id);
            throw;
        }
    }

    public async Task<Sex> AddAsync(Sex sex, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Adding sex to database: {Name}", sex.Name);
            _context.Sexes.Add(sex);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("Sex added successfully with id: {Id}", sex.Id);
            return sex;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding sex to database: {Name}", sex.Name);
            throw;
        }
    }

    public async Task<Sex> UpdateAsync(Sex sex, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Updating sex in database: {Id}", sex.Id);
            _context.Sexes.Update(sex);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("Sex updated successfully: {Id}", sex.Id);
            return sex;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating sex in database: {Id}", sex.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Deleting sex from database: {Id}", id);
            var sex = await _context.Sexes.FindAsync(new object[] { id }, cancellationToken);
            if (sex != null)
            {
                // Soft delete
                sex.IsActive = false;
                sex.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogDebug("Sex soft deleted successfully: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting sex from database: {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Checking if sex exists: {Id}", id);
            return await _context.Sexes
                .AsNoTracking()
                .AnyAsync(s => s.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if sex exists: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Sex>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Searching sexes with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync(cancellationToken);

            return await _context.Sexes
                .AsNoTracking()
                .Where(s => s.Name.Contains(searchTerm) ||
                           (s.Description != null && s.Description.Contains(searchTerm)))
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching sexes with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}