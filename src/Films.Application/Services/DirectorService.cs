using AutoMapper;
using Films.Application.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Director operations
/// </summary>
public class DirectorService : IDirectorService
{
    private readonly IDirectorRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DirectorService> _logger;

    public DirectorService(
        IDirectorRepository repository,
        IMapper mapper,
        ILogger<DirectorService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<DirectorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all directors");
            var directors = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<DirectorDto>>(directors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all directors");
            throw;
        }
    }

    public async Task<DirectorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting director with id {Id}", id);
            var director = await _repository.GetByIdAsync(id, cancellationToken);
            return director != null ? _mapper.Map<DirectorDto>(director) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting director with id {Id}", id);
            throw;
        }
    }

    public async Task<DirectorDto> CreateAsync(DirectorCreateDto directorCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new director: {Name}", directorCreateDto.Name);
            var director = _mapper.Map<Director>(directorCreateDto);
            var createdDirector = await _repository.AddAsync(director, cancellationToken);
            return _mapper.Map<DirectorDto>(createdDirector);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating director: {Name}", directorCreateDto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, DirectorUpdateDto directorUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating director with id {Id}", id);
            var existingDirector = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingDirector == null)
            {
                throw new KeyNotFoundException($"Director with id {id} not found");
            }

            _mapper.Map(directorUpdateDto, existingDirector);
            await _repository.UpdateAsync(existingDirector, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director with id {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting director with id {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting director with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<DirectorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching directors with term: {SearchTerm}", searchTerm);
            var directors = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<DirectorDto>>(directors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching directors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
