using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Actors;

public class DeleteModel : PageModel
{
    private readonly IActorService _actorService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IActorService actorService, ILogger<DeleteModel> logger)
    {
        _actorService = actorService;
        _logger = logger;
    }

    [BindProperty]
    public int Id { get; set; }

    public ActorDto? Actor { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Actor = await _actorService.GetByIdAsync(id);
            if (Actor == null)
            {
                return NotFound();
            }
            Id = id;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading actor for delete, id {Id}", id);
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _actorService.DeleteAsync(Id);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting actor, id {Id}", Id);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the actor.");
            return Page();
        }
    }
}
