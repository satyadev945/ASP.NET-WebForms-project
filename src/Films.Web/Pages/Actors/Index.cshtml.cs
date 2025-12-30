using Films.Application.Services;
using Films.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Actors;

public class IndexModel : PageModel
{
    private readonly ActorService _actorService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ActorService actorService, ILogger<IndexModel> logger)
    {
        _actorService = actorService ?? throw new ArgumentNullException(nameof(actorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IList<ActorViewModel> Actors { get; set; } = new List<ActorViewModel>();
    public string? CurrentFilter { get; set; }

    public async Task<IActionResult> OnGetAsync(string? searchString)
    {
        try
        {
            _logger.LogInformation("Loading actors page with search: {SearchString}", searchString);

            CurrentFilter = searchString;

            var actorsDto = string.IsNullOrEmpty(searchString)
                ? await _actorService.GetAllAsync()
                : await _actorService.SearchAsync(searchString);

            // Manual mapping from DTO to ViewModel
            Actors = actorsDto.Select(dto => new ActorViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate,
                IsActive = dto.IsActive,
                CreatedBy = dto.CreatedBy,
                ModifiedBy = dto.ModifiedBy
            }).ToList();

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading actors page");
            TempData["ErrorMessage"] = "An error occurred while loading actors.";
            return Page();
        }
    }
}