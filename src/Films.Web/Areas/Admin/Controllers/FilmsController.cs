using Films.Application.Interfaces;
using Films.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Films.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrator")]
public class FilmsController : Controller
{
    private readonly IFilmService _filmService;
    private readonly ILogger<FilmsController> _logger;

    public FilmsController(IFilmService filmService, ILogger<FilmsController> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var films = await _filmService.GetAllFilmsAsync();
            return View(films);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving films");
            return RedirectToAction("Error", "Home", new { area = "" });
        }
    }

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
            _logger.LogError(ex, $"Error retrieving film with ID: {id}");
            return RedirectToAction("Error", "Home", new { area = "" });
        }
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Year,Description")] Film film)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _filmService.CreateFilmAsync(film);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating film");
                ModelState.AddModelError(string.Empty, "Error creating film. Please try again.");
            }
        }
        return View(film);
    }

    public async Task<IActionResult> Edit(int id)
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
            _logger.LogError(ex, $"Error retrieving film with ID: {id} for editing");
            return RedirectToAction("Error", "Home", new { area = "" });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Year,Description")] Film film)
    {
        if (id != film.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _filmService.UpdateFilmAsync(film);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating film with ID: {id}");
                ModelState.AddModelError(string.Empty, "Error updating film. Please try again.");
            }
        }
        return View(film);
    }

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
            _logger.LogError(ex, $"Error retrieving film with ID: {id} for deletion");
            return RedirectToAction("Error", "Home", new { area = "" });
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _filmService.DeleteFilmAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting film with ID: {id}");
            return RedirectToAction("Error", "Home", new { area = "" });
        }
    }
}