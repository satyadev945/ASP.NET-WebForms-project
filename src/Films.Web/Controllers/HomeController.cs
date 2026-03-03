using Microsoft.AspNetCore.Mvc;
using Films.Web.Models;
using System.Diagnostics;

namespace Films.Web.Controllers;

/// <summary>
/// Home controller for the Films application
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Home page accessed");
        return View();
    }

    public IActionResult About()
    {
        _logger.LogInformation("About page accessed");
        return View();
    }

    public IActionResult Contact()
    {
        _logger.LogInformation("Contact page accessed");
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
