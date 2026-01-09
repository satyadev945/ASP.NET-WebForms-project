using AutoMapper;
using Films.Application.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Actor operations
/// </summary>
public class ActorService : IActorService
{
    private readonly IActorRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ActorService> _logger;

    public ActorService(
        IActorRepository repository,
        IMapper mapper,
        ILogger<ActorService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ActorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all actors");
            var actors = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ActorDto>>(actors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all actors");
            throw;
        }
    }

    public async Task<ActorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting actor with id {Id}", id);
            var actor = await _repository.GetByIdAsync(id, cancellationToken);
            return actor != null ? _mapper.Map<ActorDto>(actor) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting actor with id {Id}", id);
            throw;
        }
    }

    public async Task<ActorDto> CreateAsync(ActorCreateDto actorCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new actor: {Name}", actorCreateDto.Name);
            var actor = _mapper.Map<Actor>(actorCreateDto);
            var createdActor = await _repository.AddAsync(actor, cancellationToken);
            return _mapper.Map<ActorDto>(createdActor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating actor: {Name}", actorCreateDto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, ActorUpdateDto actorUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating actor with id {Id}", id);
            var existingActor = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingActor == null)
            {
                throw new KeyNotFoundException($"Actor with id {id} not found");
            }

            _mapper.Map(actorUpdateDto, existingActor);
            await _repository.UpdateAsync(existingActor, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating actor with id {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting actor with id {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting actor with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ActorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching actors with term: {SearchTerm}", searchTerm);
            var actors = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<ActorDto>>(actors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching actors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
