using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Directors;

public class EditModel : PageModel
{
    private readonly IDirectorService _directorService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IDirectorService directorService, ILogger<EditModel> logger)
    {
        _directorService = directorService;
        _logger = logger;
    }

    [BindProperty]
    public int Id { get; set; }

    [BindProperty]
    public DirectorInputModel Input { get; set; } = new();

    public class DirectorInputModel
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
            var director = await _directorService.GetByIdAsync(id);
            if (director == null)
            {
                return NotFound();
            }

            Id = director.Id;
            Input = new DirectorInputModel
            {
                Name = director.Name,
                Description = director.Description,
                SexId = director.SexId
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading director for edit, id {Id}", id);
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
            var dto = new DirectorUpdateDto
            {
                Name = Input.Name,
                Description = Input.Description,
                SexId = Input.SexId
            };

            await _directorService.UpdateAsync(Id, dto);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating director, id {Id}", Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the director.");
            return Page();
        }
    }
}
