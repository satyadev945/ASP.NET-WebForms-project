using Films.Application.Interfaces;
using Films.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Films.Web.Controllers;

public class HomeController : Controller
{
    private readonly IFilmService _filmService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IFilmService filmService, ILogger<HomeController> logger)
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
            _logger.LogError(ex, "Error occurred while retrieving films");
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}