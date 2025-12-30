using AutoMapper;
using Films.Application.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

public class FilmService
{
    private readonly IFilmRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<FilmService> _logger;

    public FilmService(
        IFilmRepository repository,
        IMapper mapper,
        ILogger<FilmService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

    public async Task<FilmDto> CreateAsync(FilmCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new film with name {Name}", createDto.Name);

            var film = new Film
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Genre = createDto.Genre,
                Year = createDto.Year,
                Director = createDto.Director,
                Country = createDto.Country,
                Duration = createDto.Duration,
                Rating = createDto.Rating,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System"
            };

            var createdFilm = await _repository.AddAsync(film, cancellationToken);
            _logger.LogInformation("Created film with id {Id}", createdFilm.Id);

            return _mapper.Map<FilmDto>(createdFilm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film with name {Name}", createDto.Name);
            throw;
        }
    }

    public async Task<FilmDto> UpdateAsync(int id, FilmUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating film with id {Id}", id);

            var existingFilm = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingFilm == null)
            {
                _logger.LogWarning("Film with id {Id} not found", id);
                throw new ArgumentException($"Film with id {id} not found");
            }

            existingFilm.Name = updateDto.Name;
            existingFilm.Description = updateDto.Description;
            existingFilm.Genre = updateDto.Genre;
            existingFilm.Year = updateDto.Year;
            existingFilm.Director = updateDto.Director;
            existingFilm.Country = updateDto.Country;
            existingFilm.Duration = updateDto.Duration;
            existingFilm.Rating = updateDto.Rating;
            existingFilm.ModifiedDate = DateTime.UtcNow;
            existingFilm.ModifiedBy = "System";

            var updatedFilm = await _repository.UpdateAsync(existingFilm, cancellationToken);
            _logger.LogInformation("Updated film with id {Id}", id);

            return _mapper.Map<FilmDto>(updatedFilm);
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

            var exists = await _repository.ExistsAsync(id, cancellationToken);
            if (!exists)
            {
                _logger.LogWarning("Film with id {Id} not found", id);
                throw new ArgumentException($"Film with id {id} not found");
            }

            await _repository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Deleted film with id {Id}", id);
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
            _logger.LogInformation("Searching films with term {SearchTerm}", searchTerm);
            var films = await _repository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}