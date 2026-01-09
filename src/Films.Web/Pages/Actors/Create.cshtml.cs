using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Actors;

public class CreateModel : PageModel
{
    private readonly IActorService _actorService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IActorService actorService, ILogger<CreateModel> logger)
    {
        _actorService = actorService;
        _logger = logger;
    }

    [BindProperty]
    public ActorInputModel Input { get; set; } = new();

    public class ActorInputModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public int? SexId { get; set; }
    }

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
            var dto = new ActorCreateDto
            {
                Name = Input.Name,
                Description = Input.Description,
                SexId = Input.SexId
            };

            await _actorService.CreateAsync(dto);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating actor");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the actor.");
            return Page();
        }
    }
}
