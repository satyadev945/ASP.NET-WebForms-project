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

    [BindProperty]
    public int Id { get; set; }

    public FilmDto? Film { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Film = await _filmService.GetByIdAsync(id);
            if (Film == null)
            {
                return NotFound();
            }
            Id = id;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film for delete, id {Id}", id);
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _filmService.DeleteAsync(Id);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film, id {Id}", Id);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the film.");
            return Page();
        }
    }
}
