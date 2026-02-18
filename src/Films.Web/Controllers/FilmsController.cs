using Microsoft.AspNetCore.Mvc;
using Films.Application.Interfaces;
using Films.Domain.Entities;

namespace Films.Web.Controllers;

/// <summary>
/// Controller for film management operations
/// </summary>
public class FilmsController : Controller
{
    private readonly IFilmService _filmService;
    private readonly ILogger<FilmsController> _logger;

    public FilmsController(IFilmService filmService, ILogger<FilmsController> logger)
    {
        _filmService = filmService;
        _logger = logger;
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
            TempData["Error"] = "An error occurred while loading films.";
            return View(new List<Film>());
        }
    }

    // GET: Films/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var film = await _filmService.GetFilmByIdAsync(id.Value);
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
    public async Task<IActionResult> Create([Bind("Title,Description,Year,Genre")] Film film)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _filmService.CreateFilmAsync(film);
                TempData["Success"] = "Film created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating film");
                ModelState.AddModelError("", "An error occurred while creating the film.");
            }
        }
        return View(film);
    }

    // GET: Films/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var film = await _filmService.GetFilmByIdAsync(id.Value);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film for edit, ID: {FilmId}", id);
            return NotFound();
        }
    }

    // POST: Films/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Year,Genre")] Film film)
    {
        if (id != film.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _filmService.UpdateFilmAsync(film);
                TempData["Success"] = "Film updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating film with ID: {FilmId}", id);
                ModelState.AddModelError("", "An error occurred while updating the film.");
            }
        }
        return View(film);
    }

    // GET: Films/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var film = await _filmService.GetFilmByIdAsync(id.Value);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film for delete, ID: {FilmId}", id);
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
            var result = await _filmService.DeleteFilmAsync(id);
            if (result)
            {
                TempData["Success"] = "Film deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Film not found.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film with ID: {FilmId}", id);
            TempData["Error"] = "An error occurred while deleting the film.";
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Films/Search
    public async Task<IActionResult> Search(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var films = await _filmService.SearchFilmsAsync(searchTerm);
            ViewData["SearchTerm"] = searchTerm;
            return View("Index", films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching films with term: {SearchTerm}", searchTerm);
            TempData["Error"] = "An error occurred while searching films.";
            return RedirectToAction(nameof(Index));
        }
    }
}
