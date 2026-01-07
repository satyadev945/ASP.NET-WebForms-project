using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Films.Application.DTOs;
using Films.Application.Interfaces;

namespace Films.Web.Pages.FilmPages;

public class DetailsModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IFilmService filmService, ILogger<DetailsModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

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
}
