using Films.Application.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for actor operations
/// </summary>
public class ActorService : IActorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ActorService> _logger;

    public ActorService(IUnitOfWork unitOfWork, ILogger<ActorService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<ActorDto>> GetAllActorsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all actors");
            var actors = await _unitOfWork.Actors.GetAllAsync();
            return actors.Select(a => new ActorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                SexId = a.SexId,
                BirthDate = a.BirthDate,
                Biography = a.Biography
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all actors");
            throw;
        }
    }

    public async Task<ActorDto?> GetActorByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving actor with ID: {ActorId}", id);
            var actor = await _unitOfWork.Actors.GetByIdAsync(id);
            if (actor == null)
            {
                _logger.LogWarning("Actor with ID {ActorId} not found", id);
                return null;
            }

            return new ActorDto
            {
                Id = actor.Id,
                FirstName = actor.FirstName,
                LastName = actor.LastName,
                SexId = actor.SexId,
                BirthDate = actor.BirthDate,
                Biography = actor.Biography
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving actor with ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<ActorDto> CreateActorAsync(CreateActorDto createActorDto)
    {
        try
        {
            _logger.LogInformation("Creating new actor: {FirstName} {LastName}", 
                createActorDto.FirstName, createActorDto.LastName);
            
            var actor = new Actor
            {
                FirstName = createActorDto.FirstName,
                LastName = createActorDto.LastName,
                SexId = createActorDto.SexId,
                BirthDate = createActorDto.BirthDate,
                Biography = createActorDto.Biography,
                CreatedDate = DateTime.UtcNow
            };

            var createdActor = await _unitOfWork.Actors.AddAsync(actor);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Actor created successfully with ID: {ActorId}", createdActor.Id);

            return new ActorDto
            {
                Id = createdActor.Id,
                FirstName = createdActor.FirstName,
                LastName = createdActor.LastName,
                SexId = createdActor.SexId,
                BirthDate = createdActor.BirthDate,
                Biography = createdActor.Biography
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating actor: {FirstName} {LastName}", 
                createActorDto.FirstName, createActorDto.LastName);
            throw;
        }
    }

    public async Task UpdateActorAsync(UpdateActorDto updateActorDto)
    {
        try
        {
            _logger.LogInformation("Updating actor with ID: {ActorId}", updateActorDto.Id);
            
            var actor = await _unitOfWork.Actors.GetByIdAsync(updateActorDto.Id);
            if (actor == null)
            {
                _logger.LogWarning("Actor with ID {ActorId} not found for update", updateActorDto.Id);
                throw new InvalidOperationException($"Actor with ID {updateActorDto.Id} not found");
            }

            actor.FirstName = updateActorDto.FirstName;
            actor.LastName = updateActorDto.LastName;
            actor.SexId = updateActorDto.SexId;
            actor.BirthDate = updateActorDto.BirthDate;
            actor.Biography = updateActorDto.Biography;
            actor.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.Actors.UpdateAsync(actor);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Actor updated successfully: {ActorId}", updateActorDto.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating actor with ID: {ActorId}", updateActorDto.Id);
            throw;
        }
    }

    public async Task DeleteActorAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting actor with ID: {ActorId}", id);
            
            var exists = await _unitOfWork.Actors.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Actor with ID {ActorId} not found for deletion", id);
                throw new InvalidOperationException($"Actor with ID {id} not found");
            }

            await _unitOfWork.Actors.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Actor deleted successfully: {ActorId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting actor with ID: {ActorId}", id);
            throw;
        }
    }
}
