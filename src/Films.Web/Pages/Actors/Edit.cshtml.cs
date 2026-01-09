using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Actors;

public class EditModel : PageModel
{
    private readonly IActorService _actorService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IActorService actorService, ILogger<EditModel> logger)
    {
        _actorService = actorService;
        _logger = logger;
    }

    [BindProperty]
    public int Id { get; set; }

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

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var actor = await _actorService.GetByIdAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            Id = actor.Id;
            Input = new ActorInputModel
            {
                Name = actor.Name,
                Description = actor.Description,
                SexId = actor.SexId
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading actor for edit, id {Id}", id);
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var dto = new ActorUpdateDto
            {
                Name = Input.Name,
                Description = Input.Description,
                SexId = Input.SexId
            };

            await _actorService.UpdateAsync(Id, dto);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating actor, id {Id}", Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the actor.");
            return Page();
        }
    }
}
