using Microsoft.AspNetCore.Mvc;
using Films.Application.Interfaces;

namespace Films.Web.Controllers;

/// <summary>
/// Home controller for the Films application
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IFilmService _filmService;

    public HomeController(ILogger<HomeController> logger, IFilmService filmService)
    {
        _logger = logger;
        _filmService = filmService;
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
            _logger.LogError(ex, "Error loading home page");
            return View("Error");
        }
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
