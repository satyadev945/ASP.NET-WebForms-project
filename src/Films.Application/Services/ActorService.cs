using AutoMapper;
using Microsoft.Extensions.Logging;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;

namespace Films.Application.Services;

public class ActorService : IActorService
{
    private readonly IActorRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ActorService> _logger;

    public ActorService(IActorRepository repository, IMapper mapper, ILogger<ActorService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<ActorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var actors = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ActorDto>>(actors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync");
            throw;
        }
    }

    public async Task<ActorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var actor = await _repository.GetByIdAsync(id, cancellationToken);
            return actor != null ? _mapper.Map<ActorDto>(actor) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetByIdAsync for ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<ActorDto> CreateAsync(ActorCreateDto actorCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var actor = _mapper.Map<Actor>(actorCreateDto);
            var addedActor = await _repository.AddAsync(actor, cancellationToken);
            return _mapper.Map<ActorDto>(addedActor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync");
            throw;
        }
    }

    public async Task UpdateAsync(int id, ActorUpdateDto actorUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingActor = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingActor == null)
            {
                throw new InvalidOperationException($"Actor with ID {id} not found");
            }

            _mapper.Map(actorUpdateDto, existingActor);
            await _repository.UpdateAsync(existingActor, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateAsync for ID: {ActorId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteAsync for ID: {ActorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ActorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var actors = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<ActorDto>>(actors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SearchAsync with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
