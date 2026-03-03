using Microsoft.AspNetCore.Mvc;
using Films.Application.Services;
using Films.Application.DTOs;

namespace Films.Web.Controllers;

/// <summary>
/// Controller for managing films
/// </summary>
public class FilmsController : Controller
{
    private readonly IFilmService _filmService;
    private readonly ILogger<FilmsController> _logger;

    public FilmsController(IFilmService filmService, ILogger<FilmsController> logger)
    {
        _filmService = filmService ?? throw new ArgumentNullException(nameof(filmService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Films
    public async Task<IActionResult> Index()
    {
        try
        {
            var films = await _filmService.GetAllFilmsAsync();
            return View(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving films list");
            TempData["Error"] = "An error occurred while retrieving films.";
            return View(new List<FilmDto>());
        }
    }

    // GET: Films/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var film = await _filmService.GetFilmByIdAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving film details for ID: {FilmId}", id);
            return NotFound();
        }
    }

    // GET: Films/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Films/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateFilmDto createFilmDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createFilmDto);
        }

        try
        {
            await _filmService.CreateFilmAsync(createFilmDto);
            TempData["Success"] = "Film created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film");
            ModelState.AddModelError("", "An error occurred while creating the film.");
            return View(createFilmDto);
        }
    }

    // GET: Films/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var film = await _filmService.GetFilmByIdAsync(id);
            if (film == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateFilmDto
            {
                Id = film.Id,
                Title = film.Title,
                Description = film.Description,
                Year = film.Year,
                Genre = film.Genre
            };

            return View(updateDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving film for edit: {FilmId}", id);
            return NotFound();
        }
    }

    // POST: Films/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateFilmDto updateFilmDto)
    {
        if (id != updateFilmDto.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(updateFilmDto);
        }

        try
        {
            await _filmService.UpdateFilmAsync(updateFilmDto);
            TempData["Success"] = "Film updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film: {FilmId}", id);
            ModelState.AddModelError("", "An error occurred while updating the film.");
            return View(updateFilmDto);
        }
    }

    // GET: Films/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var film = await _filmService.GetFilmByIdAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving film for deletion: {FilmId}", id);
            return NotFound();
        }
    }

    // POST: Films/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _filmService.DeleteFilmAsync(id);
            TempData["Success"] = "Film deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film: {FilmId}", id);
            TempData["Error"] = "An error occurred while deleting the film.";
            return RedirectToAction(nameof(Index));
        }
    }
}
