using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Films;

public class EditModel : PageModel
{
    private readonly IFilmService _filmService;

    public EditModel(IFilmService filmService)
    {
        _filmService = filmService;
    }

    [BindProperty]
    public Film Film { get; set; } = new Film();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var film = await _filmService.GetByIdAsync(id);

        if (film == null)
        {
            return NotFound();
        }

        Film = film;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _filmService.UpdateAsync(Film.Id, Film);

        return RedirectToPage("./Index");
    }
}
