using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Films;

public class DeleteModel : PageModel
{
    private readonly IFilmService _filmService;

    public DeleteModel(IFilmService filmService)
    {
        _filmService = filmService;
    }

    [BindProperty]
    public Film? Film { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Film = await _filmService.GetByIdAsync(id);

        if (Film == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await _filmService.DeleteAsync(id);
        return RedirectToPage("./Index");
    }
}
