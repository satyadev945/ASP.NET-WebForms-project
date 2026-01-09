using AutoMapper;
using Films.Application.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for Film operations
/// </summary>
public class FilmService : IFilmService
{
    private readonly IFilmRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<FilmService> _logger;

    public FilmService(
        IFilmRepository repository,
        IMapper mapper,
        ILogger<FilmService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<FilmDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all films");
            var films = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all films");
            throw;
        }
    }

    public async Task<FilmDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting film with id {Id}", id);
            var film = await _repository.GetByIdAsync(id, cancellationToken);
            return film != null ? _mapper.Map<FilmDto>(film) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting film with id {Id}", id);
            throw;
        }
    }

    public async Task<FilmDto> CreateAsync(FilmCreateDto filmCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new film: {Name}", filmCreateDto.Name);
            var film = _mapper.Map<Film>(filmCreateDto);
            var createdFilm = await _repository.AddAsync(film, cancellationToken);
            return _mapper.Map<FilmDto>(createdFilm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film: {Name}", filmCreateDto.Name);
            throw;
        }
    }

    public async Task UpdateAsync(int id, FilmUpdateDto filmUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating film with id {Id}", id);
            var existingFilm = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingFilm == null)
            {
                throw new KeyNotFoundException($"Film with id {id} not found");
            }

            _mapper.Map(filmUpdateDto, existingFilm);
            await _repository.UpdateAsync(existingFilm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film with id {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting film with id {Id}", id);
            await _repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<FilmDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching films with term: {SearchTerm}", searchTerm);
            var films = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
