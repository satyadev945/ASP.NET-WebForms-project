using Films.Application.Interfaces;
using Films.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Films.Web.Controllers;

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
            return RedirectToAction("Error", "Home");
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
            return RedirectToAction("Error", "Home");
        }
    }
}