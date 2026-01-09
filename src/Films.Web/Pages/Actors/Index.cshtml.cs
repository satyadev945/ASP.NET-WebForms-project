using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Actors;

public class IndexModel : PageModel
{
    private readonly IActorService _actorService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IActorService actorService, ILogger<IndexModel> logger)
    {
        _actorService = actorService;
        _logger = logger;
    }

    public IEnumerable<ActorDto> Actors { get; set; } = new List<ActorDto>();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Actors = await _actorService.GetAllAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading actors");
            ErrorMessage = "An error occurred while loading actors.";
            return Page();
        }
    }
}
