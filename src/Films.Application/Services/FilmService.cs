using AutoMapper;
using Microsoft.Extensions.Logging;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;

namespace Films.Application.Services;

public class FilmService : IFilmService
{
    private readonly IFilmRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<FilmService> _logger;

    public FilmService(IFilmRepository repository, IMapper mapper, ILogger<FilmService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<FilmDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var films = await _repository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync");
            throw;
        }
    }

    public async Task<FilmDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var film = await _repository.GetByIdAsync(id, cancellationToken);
            return film != null ? _mapper.Map<FilmDto>(film) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetByIdAsync for ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<FilmDto> CreateAsync(FilmCreateDto filmCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var film = _mapper.Map<Film>(filmCreateDto);
            var addedFilm = await _repository.AddAsync(film, cancellationToken);
            return _mapper.Map<FilmDto>(addedFilm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync");
            throw;
        }
    }

    public async Task UpdateAsync(int id, FilmUpdateDto filmUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingFilm = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingFilm == null)
            {
                throw new InvalidOperationException($"Film with ID {id} not found");
            }

            _mapper.Map(filmUpdateDto, existingFilm);
            await _repository.UpdateAsync(existingFilm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateAsync for ID: {FilmId}", id);
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
            _logger.LogError(ex, "Error in DeleteAsync for ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<FilmDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var films = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SearchAsync with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
