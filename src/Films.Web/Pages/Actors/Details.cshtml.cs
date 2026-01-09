using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Actors;

public class DetailsModel : PageModel
{
    private readonly IActorService _actorService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IActorService actorService, ILogger<DetailsModel> logger)
    {
        _actorService = actorService;
        _logger = logger;
    }

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
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading actor details for id {Id}", id);
            return RedirectToPage("Index");
        }
    }
}
