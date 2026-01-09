using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Directors;

public class DeleteModel : PageModel
{
    private readonly IDirectorService _directorService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IDirectorService directorService, ILogger<DeleteModel> logger)
    {
        _directorService = directorService;
        _logger = logger;
    }

    [BindProperty]
    public int Id { get; set; }

    public DirectorDto? Director { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Director = await _directorService.GetByIdAsync(id);
            if (Director == null)
            {
                return NotFound();
            }
            Id = id;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading director for delete, id {Id}", id);
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _directorService.DeleteAsync(Id);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting director, id {Id}", Id);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the director.");
            return Page();
        }
    }
}
