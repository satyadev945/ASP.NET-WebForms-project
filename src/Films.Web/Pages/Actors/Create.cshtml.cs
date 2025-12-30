using Films.Application.DTOs;
using Films.Application.Services;
using Films.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Actors;

public class CreateModel : PageModel
{
    private readonly ActorService _actorService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(ActorService actorService, ILogger<CreateModel> logger)
    {
        _actorService = actorService ?? throw new ArgumentNullException(nameof(actorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public ActorCreateViewModel Actor { get; set; } = new();

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            _logger.LogInformation("Creating new actor: {Name}", Actor.Name);

            // Manual mapping from ViewModel to DTO
            var createDto = new ActorCreateDto
            {
                Name = Actor.Name,
                Description = Actor.Description
            };

            await _actorService.CreateAsync(createDto);

            _logger.LogInformation("Successfully created actor: {Name}", Actor.Name);
            TempData["SuccessMessage"] = $"Actor '{Actor.Name}' has been created successfully.";

            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating actor: {Name}", Actor.Name);
            ModelState.AddModelError(string.Empty, "An error occurred while creating the actor. Please try again.");
            return Page();
        }
    }
}