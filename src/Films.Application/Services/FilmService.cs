using AutoMapper;
using Films.Domain.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Films.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Film service implementation
/// </summary>
public class FilmService : IFilmService
{
    private readonly IFilmRepository _filmRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<FilmService> _logger;

    public FilmService(
        IFilmRepository filmRepository,
        IMapper mapper,
        ILogger<FilmService> logger)
    {
        _filmRepository = filmRepository ?? throw new ArgumentNullException(nameof(filmRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<FilmDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting all films");
            var films = await _filmRepository.GetAllAsync(cancellationToken);
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
            _logger.LogInformation("Getting film by id: {Id}", id);
            var film = await _filmRepository.GetByIdAsync(id, cancellationToken);
            return film != null ? _mapper.Map<FilmDto>(film) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting film by id: {Id}", id);
            throw;
        }
    }

    public async Task<FilmDto> CreateAsync(FilmCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating film: {Name}", dto.Name);

            var film = _mapper.Map<Film>(dto);
            film.CreatedDate = DateTime.UtcNow;
            film.IsActive = true;

            var createdFilm = await _filmRepository.AddAsync(film, cancellationToken);

            _logger.LogInformation("Film created successfully with id: {Id}", createdFilm.Id);
            return _mapper.Map<FilmDto>(createdFilm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film: {Name}", dto.Name);
            throw;
        }
    }

    public async Task<FilmDto> UpdateAsync(int id, FilmUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating film: {Id}", id);

            var existingFilm = await _filmRepository.GetByIdAsync(id, cancellationToken);
            if (existingFilm == null)
            {
                _logger.LogWarning("Film not found for update: {Id}", id);
                throw new ArgumentException($"Film with id {id} not found", nameof(id));
            }

            _mapper.Map(dto, existingFilm);
            existingFilm.ModifiedDate = DateTime.UtcNow;

            var updatedFilm = await _filmRepository.UpdateAsync(existingFilm, cancellationToken);

            _logger.LogInformation("Film updated successfully: {Id}", id);
            return _mapper.Map<FilmDto>(updatedFilm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film: {Id}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting film: {Id}", id);

            var exists = await _filmRepository.ExistsAsync(id, cancellationToken);
            if (!exists)
            {
                _logger.LogWarning("Film not found for deletion: {Id}", id);
                throw new ArgumentException($"Film with id {id} not found", nameof(id));
            }

            await _filmRepository.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("Film deleted successfully: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<FilmDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching films with term: {SearchTerm}", searchTerm);
            var films = await _filmRepository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<FilmDto>> GetByActorAsync(int actorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting films by actor: {ActorId}", actorId);
            var films = await _filmRepository.GetByActorAsync(actorId, cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting films by actor: {ActorId}", actorId);
            throw;
        }
    }

    public async Task<IEnumerable<FilmDto>> GetByDirectorAsync(int directorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting films by director: {DirectorId}", directorId);
            var films = await _filmRepository.GetByDirectorAsync(directorId, cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting films by director: {DirectorId}", directorId);
            throw;
        }
    }

    public async Task<IEnumerable<FilmDto>> GetByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting films by year: {Year}", year);
            var films = await _filmRepository.GetByYearAsync(year, cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting films by year: {Year}", year);
            throw;
        }
    }

    public async Task<IEnumerable<FilmDto>> GetByGenreAsync(string genre, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting films by genre: {Genre}", genre);
            var films = await _filmRepository.GetByGenreAsync(genre, cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting films by genre: {Genre}", genre);
            throw;
        }
    }
}