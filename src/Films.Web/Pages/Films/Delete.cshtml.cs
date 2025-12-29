using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Films;

public class DeleteModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IFilmService filmService, ILogger<DeleteModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public FilmDto? Film { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Film = await _filmService.GetByIdAsync(Id);

            if (Film == null)
            {
                return NotFound();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film for deletion with id {Id}", Id);
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _filmService.DeleteAsync(Id);

            TempData["SuccessMessage"] = "Филмът е изтрит успешно";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film with id {Id}", Id);
            TempData["ErrorMessage"] = "Възникна грешка при изтриването на филма";
            return RedirectToPage("./Index");
        }
    }
}
