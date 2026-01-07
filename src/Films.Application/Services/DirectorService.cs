using AutoMapper;
using Microsoft.Extensions.Logging;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;

namespace Films.Application.Services;

public class DirectorService : IDirectorService
{
    private readonly IDirectorRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DirectorService> _logger;

    public DirectorService(IDirectorRepository repository, IMapper mapper, ILogger<DirectorService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<DirectorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var directors = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<DirectorDto>>(directors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync");
            throw;
        }
    }

    public async Task<DirectorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var director = await _repository.GetByIdAsync(id, cancellationToken);
            return director != null ? _mapper.Map<DirectorDto>(director) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetByIdAsync for ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<DirectorDto> CreateAsync(DirectorCreateDto directorCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var director = _mapper.Map<Director>(directorCreateDto);
            var addedDirector = await _repository.AddAsync(director, cancellationToken);
            return _mapper.Map<DirectorDto>(addedDirector);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync");
            throw;
        }
    }

    public async Task UpdateAsync(int id, DirectorUpdateDto directorUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingDirector = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingDirector == null)
            {
                throw new InvalidOperationException($"Director with ID {id} not found");
            }

            _mapper.Map(directorUpdateDto, existingDirector);
            await _repository.UpdateAsync(existingDirector, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateAsync for ID: {DirectorId}", id);
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
            _logger.LogError(ex, "Error in DeleteAsync for ID: {DirectorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<DirectorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var directors = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<DirectorDto>>(directors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SearchAsync with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
