using Films.Application.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Films.Application.Services;

/// <summary>
/// Service implementation for film operations
/// </summary>
public class FilmService : IFilmService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FilmService> _logger;

    public FilmService(IUnitOfWork unitOfWork, ILogger<FilmService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<FilmDto>> GetAllFilmsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all films");
            var films = await _unitOfWork.Films.GetAllAsync();
            return films.Select(f => new FilmDto
            {
                Id = f.Id,
                Title = f.Title,
                Description = f.Description,
                Year = f.Year,
                Genre = f.Genre
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all films");
            throw;
        }
    }

    public async Task<FilmDto?> GetFilmByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving film with ID: {FilmId}", id);
            var film = await _unitOfWork.Films.GetByIdAsync(id);
            if (film == null)
            {
                _logger.LogWarning("Film with ID {FilmId} not found", id);
                return null;
            }

            return new FilmDto
            {
                Id = film.Id,
                Title = film.Title,
                Description = film.Description,
                Year = film.Year,
                Genre = film.Genre
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving film with ID: {FilmId}", id);
            throw;
        }
    }

    public async Task<FilmDto> CreateFilmAsync(CreateFilmDto createFilmDto)
    {
        try
        {
            _logger.LogInformation("Creating new film: {Title}", createFilmDto.Title);
            
            var film = new Film
            {
                Title = createFilmDto.Title,
                Description = createFilmDto.Description,
                Year = createFilmDto.Year,
                Genre = createFilmDto.Genre,
                CreatedDate = DateTime.UtcNow
            };

            var createdFilm = await _unitOfWork.Films.AddAsync(film);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Film created successfully with ID: {FilmId}", createdFilm.Id);

            return new FilmDto
            {
                Id = createdFilm.Id,
                Title = createdFilm.Title,
                Description = createdFilm.Description,
                Year = createdFilm.Year,
                Genre = createdFilm.Genre
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film: {Title}", createFilmDto.Title);
            throw;
        }
    }

    public async Task UpdateFilmAsync(UpdateFilmDto updateFilmDto)
    {
        try
        {
            _logger.LogInformation("Updating film with ID: {FilmId}", updateFilmDto.Id);
            
            var film = await _unitOfWork.Films.GetByIdAsync(updateFilmDto.Id);
            if (film == null)
            {
                _logger.LogWarning("Film with ID {FilmId} not found for update", updateFilmDto.Id);
                throw new InvalidOperationException($"Film with ID {updateFilmDto.Id} not found");
            }

            film.Title = updateFilmDto.Title;
            film.Description = updateFilmDto.Description;
            film.Year = updateFilmDto.Year;
            film.Genre = updateFilmDto.Genre;
            film.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.Films.UpdateAsync(film);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Film updated successfully: {FilmId}", updateFilmDto.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film with ID: {FilmId}", updateFilmDto.Id);
            throw;
        }
    }

    public async Task DeleteFilmAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting film with ID: {FilmId}", id);
            
            var exists = await _unitOfWork.Films.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Film with ID {FilmId} not found for deletion", id);
                throw new InvalidOperationException($"Film with ID {id} not found");
            }

            await _unitOfWork.Films.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Film deleted successfully: {FilmId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film with ID: {FilmId}", id);
            throw;
        }
    }
}
