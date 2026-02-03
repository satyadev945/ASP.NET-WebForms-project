using Films.Application.Interfaces;
using Films.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Films.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrator")]
public class ActorsController : Controller
{
    private readonly IActorService _actorService;
    private readonly ILogger<ActorsController> _logger;

    public ActorsController(IActorService actorService, ILogger<ActorsController> logger)
    {
        _actorService = actorService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var actors = await _actorService.GetAllActorsAsync();
            return View(actors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving actors");
            return RedirectToAction("Error", "Home", new { area = "" });
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var actor = await _actorService.GetActorByIdAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving actor with ID: {id}");
            return RedirectToAction("Error", "Home", new { area = "" });
        }
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,DateOfBirth,SexId")] Actor actor)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _actorService.CreateActorAsync(actor);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating actor");
                ModelState.AddModelError(string.Empty, "Error creating actor. Please try again.");
            }
        }
        return View(actor);
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var actor = await _actorService.GetActorByIdAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving actor with ID: {id} for editing");
            return RedirectToAction("Error", "Home", new { area = "" });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DateOfBirth,SexId")] Actor actor)
    {
        if (id != actor.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _actorService.UpdateActorAsync(actor);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating actor with ID: {id}");
                ModelState.AddModelError(string.Empty, "Error updating actor. Please try again.");
            }
        }
        return View(actor);
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var actor = await _actorService.GetActorByIdAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving actor with ID: {id} for deletion");
            return RedirectToAction("Error", "Home", new { area = "" });
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _actorService.DeleteActorAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting actor with ID: {id}");
            return RedirectToAction("Error", "Home", new { area = "" });
        }
    }
}