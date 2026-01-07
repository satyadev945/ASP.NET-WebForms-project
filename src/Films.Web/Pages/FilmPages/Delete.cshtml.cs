using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Films.Application.DTOs;
using Films.Application.Interfaces;

namespace Films.Web.Pages.FilmPages;

public class DeleteModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IFilmService filmService, ILogger<DeleteModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    [BindProperty]
    public FilmDto Film { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var film = await _filmService.GetByIdAsync(id);
            if (film == null)
            {
                return NotFound();
            }

            Film = film;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film {FilmId}", id);
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _filmService.DeleteAsync(Film.Id);
            TempData["SuccessMessage"] = "Film deleted successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film {FilmId}", Film.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the film.");
            return Page();
        }
    }
}
