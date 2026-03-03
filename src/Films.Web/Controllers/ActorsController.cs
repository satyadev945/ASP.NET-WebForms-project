using Microsoft.AspNetCore.Mvc;
using Films.Application.Services;
using Films.Application.DTOs;

namespace Films.Web.Controllers;

/// <summary>
/// Controller for managing actors
/// </summary>
public class ActorsController : Controller
{
    private readonly IActorService _actorService;
    private readonly ILogger<ActorsController> _logger;

    public ActorsController(IActorService actorService, ILogger<ActorsController> logger)
    {
        _actorService = actorService ?? throw new ArgumentNullException(nameof(actorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Actors
    public async Task<IActionResult> Index()
    {
        try
        {
            var actors = await _actorService.GetAllActorsAsync();
            return View(actors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving actors list");
            TempData["Error"] = "An error occurred while retrieving actors.";
            return View(new List<ActorDto>());
        }
    }

    // GET: Actors/Details/5
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
            _logger.LogError(ex, "Error retrieving actor details for ID: {ActorId}", id);
            return NotFound();
        }
    }

    // GET: Actors/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Actors/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateActorDto createActorDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createActorDto);
        }

        try
        {
            await _actorService.CreateActorAsync(createActorDto);
            TempData["Success"] = "Actor created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating actor");
            ModelState.AddModelError("", "An error occurred while creating the actor.");
            return View(createActorDto);
        }
    }

    // GET: Actors/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var actor = await _actorService.GetActorByIdAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateActorDto
            {
                Id = actor.Id,
                FirstName = actor.FirstName,
                LastName = actor.LastName,
                SexId = actor.SexId,
                BirthDate = actor.BirthDate,
                Biography = actor.Biography
            };

            return View(updateDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving actor for edit: {ActorId}", id);
            return NotFound();
        }
    }

    // POST: Actors/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateActorDto updateActorDto)
    {
        if (id != updateActorDto.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(updateActorDto);
        }

        try
        {
            await _actorService.UpdateActorAsync(updateActorDto);
            TempData["Success"] = "Actor updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating actor: {ActorId}", id);
            ModelState.AddModelError("", "An error occurred while updating the actor.");
            return View(updateActorDto);
        }
    }

    // GET: Actors/Delete/5
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
            _logger.LogError(ex, "Error retrieving actor for deletion: {ActorId}", id);
            return NotFound();
        }
    }

    // POST: Actors/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _actorService.DeleteActorAsync(id);
            TempData["Success"] = "Actor deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting actor: {ActorId}", id);
            TempData["Error"] = "An error occurred while deleting the actor.";
            return RedirectToAction(nameof(Index));
        }
    }
}
