using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Actor entity
/// </summary>
public class ActorService : IActorService
{
    private readonly IActorRepository _actorRepository;
    private readonly ILogger<ActorService> _logger;

    public ActorService(IActorRepository actorRepository, ILogger<ActorService> logger)
    {
        _actorRepository = actorRepository ?? throw new ArgumentNullException(nameof(actorRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Actor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching all actors");
            return await _actorRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all actors");
            throw;
        }
    }

    public async Task<Actor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching actor with ID: {ActorId}", id);
            return await _actorRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<Actor> CreateAsync(Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new actor: {ActorName}", actor.Name);
            actor.CreatedDate = DateTime.UtcNow;
            actor.IsActive = true;
            actor.CreatedBy = "System";
            return await _actorRepository.AddAsync(actor, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating actor: {ActorName}", actor.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, Actor actor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating actor with ID: {ActorId}", id);
            var existing = await _actorRepository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Actor with ID {id} not found");
            }

            actor.Id = id;
            actor.ModifiedDate = DateTime.UtcNow;
            actor.ModifiedBy = "System";
            actor.CreatedDate = existing.CreatedDate;
            actor.CreatedBy = existing.CreatedBy;

            await _actorRepository.UpdateAsync(actor, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting actor with ID: {ActorId}", id);
            await _actorRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Actor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching actors with term: {SearchTerm}", searchTerm);
            return await _actorRepository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching actors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
