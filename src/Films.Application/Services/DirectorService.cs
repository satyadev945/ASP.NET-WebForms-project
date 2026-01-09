using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Director entity
/// </summary>
public class DirectorService : IDirectorService
{
    private readonly IDirectorRepository _directorRepository;
    private readonly ILogger<DirectorService> _logger;

    public DirectorService(IDirectorRepository directorRepository, ILogger<DirectorService> logger)
    {
        _directorRepository = directorRepository ?? throw new ArgumentNullException(nameof(directorRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<DirectedBy>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching all directors");
            return await _directorRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all directors");
            throw;
        }
    }

    public async Task<DirectedBy?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching director with ID: {DirectorId}", id);
            return await _directorRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<DirectedBy> CreateAsync(DirectedBy director, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new director: {DirectorName}", director.Name);
            director.CreatedDate = DateTime.UtcNow;
            director.IsActive = true;
            director.CreatedBy = "System";
            return await _directorRepository.AddAsync(director, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating director: {DirectorName}", director.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, DirectedBy director, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating director with ID: {DirectorId}", id);
            var existing = await _directorRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Director with ID {id} not found");
            }

            director.Id = id;
            director.ModifiedDate = DateTime.UtcNow;
            director.ModifiedBy = "System";
            director.CreatedDate = existing.CreatedDate;
            director.CreatedBy = existing.CreatedBy;

            await _directorRepository.UpdateAsync(director, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting director with ID: {DirectorId}", id);
            await _directorRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting director with ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<DirectedBy>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching directors with term: {SearchTerm}", searchTerm);
            return await _directorRepository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching directors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
