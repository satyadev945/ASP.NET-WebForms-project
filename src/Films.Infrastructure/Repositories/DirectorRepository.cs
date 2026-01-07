using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Infrastructure.Data;

namespace Films.Infrastructure.Repositories;

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
            return await _context.Directors
                .Include(d => d.Sex)
                .AsNoTracking()
                .Where(d => d.IsActive)
                .OrderBy(d => d.LastName).ThenBy(d => d.FirstName)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all directors");
            throw;
        }
    }

    public async Task<Director?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Directors
                .Include(d => d.Sex)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<Director> AddAsync(Director director, CancellationToken cancellationToken = default)
    {
        try
        {
            director.CreatedDate = DateTime.UtcNow;
            director.IsActive = true;
            await _context.Directors.AddAsync(director, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return director;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding director: {FirstName} {LastName}", director.FirstName, director.LastName);
            throw;
        }
    }

    public async Task UpdateAsync(Director director, CancellationToken cancellationToken = default)
    {
        try
        {
            director.ModifiedDate = DateTime.UtcNow;
            _context.Directors.Update(director);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director with ID: {DirectorId}", director.Id);
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
            _logger.LogError(ex, "Error deleting director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Directors.AsNoTracking().AnyAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if director exists with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Director>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Directors
                .Include(d => d.Sex)
                .AsNoTracking()
                .Where(d => d.IsActive && (d.FirstName.Contains(searchTerm) || d.LastName.Contains(searchTerm)))
                .OrderBy(d => d.LastName).ThenBy(d => d.FirstName)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching directors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
