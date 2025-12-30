using AutoMapper;
using Films.Application.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

public class ActorService
{
    private readonly IActorRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ActorService> _logger;

    public ActorService(
        IActorRepository repository,
        IMapper mapper,
        ILogger<ActorService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

    public async Task<ActorDto> CreateAsync(ActorCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new actor with name {Name}", createDto.Name);

            var actor = new Actor
            {
                Name = createDto.Name,
                Description = createDto.Description,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System"
            };

            var createdActor = await _repository.AddAsync(actor, cancellationToken);
            _logger.LogInformation("Created actor with id {Id}", createdActor.Id);

            return _mapper.Map<ActorDto>(createdActor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating actor with name {Name}", createDto.Name);
            throw;
        }
    }

    public async Task<ActorDto> UpdateAsync(int id, ActorUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating actor with id {Id}", id);

            var existingActor = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingActor == null)
            {
                _logger.LogWarning("Actor with id {Id} not found", id);
                throw new ArgumentException($"Actor with id {id} not found");
            }

            existingActor.Name = updateDto.Name;
            existingActor.Description = updateDto.Description;
            existingActor.ModifiedDate = DateTime.UtcNow;
            existingActor.ModifiedBy = "System";

            var updatedActor = await _repository.UpdateAsync(existingActor, cancellationToken);
            _logger.LogInformation("Updated actor with id {Id}", id);

            return _mapper.Map<ActorDto>(updatedActor);
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

            var exists = await _repository.ExistsAsync(id, cancellationToken);
            if (!exists)
            {
                _logger.LogWarning("Actor with id {Id} not found", id);
                throw new ArgumentException($"Actor with id {id} not found");
            }

            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Deleted actor with id {Id}", id);
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
            _logger.LogInformation("Searching actors with term {SearchTerm}", searchTerm);
            var actors = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<ActorDto>>(actors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching actors with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}