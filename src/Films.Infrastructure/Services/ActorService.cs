using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Films.Infrastructure.Services;

/// <summary>
/// Service implementation for actor operations
/// </summary>
public class ActorService : IActorService
{
    private readonly FilmsDbContext _context;
    private readonly ILogger<ActorService> _logger;

    public ActorService(FilmsDbContext context, ILogger<ActorService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Actor>> GetAllActorsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all actors");
            return await _context.Actors
                .Include(a => a.Sex)
                .Include(a => a.ActorFilms)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all actors");
            throw;
        }
    }

    public async Task<Actor?> GetActorByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving actor with ID: {ActorId}", id);
            return await _context.Actors
                .Include(a => a.Sex)
                .Include(a => a.ActorFilms)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<Actor> CreateActorAsync(Actor actor)
    {
        try
        {
            _logger.LogInformation("Creating new actor: {ActorName}", $"{actor.FirstName} {actor.LastName}");
            actor.CreatedDate = DateTime.UtcNow;
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Actor created successfully with ID: {ActorId}", actor.Id);
            return actor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating actor: {ActorName}", $"{actor.FirstName} {actor.LastName}");
            throw;
        }
    }

    public async Task<Actor> UpdateActorAsync(Actor actor)
    {
        try
        {
            _logger.LogInformation("Updating actor with ID: {ActorId}", actor.Id);
            actor.ModifiedDate = DateTime.UtcNow;
            _context.Actors.Update(actor);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Actor updated successfully with ID: {ActorId}", actor.Id);
            return actor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating actor with ID: {ActorId}", actor.Id);
            throw;
        }
    }

    public async Task<bool> DeleteActorAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting actor with ID: {ActorId}", id);
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                _logger.LogWarning("Actor with ID: {ActorId} not found", id);
                return false;
            }

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Actor deleted successfully with ID: {ActorId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Actor>> SearchActorsAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation("Searching actors with term: {SearchTerm}", searchTerm);
            return await _context.Actors
                .Where(a => a.FirstName.Contains(searchTerm) || a.LastName.Contains(searchTerm))
                .Include(a => a.Sex)
                .Include(a => a.ActorFilms)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching actors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
