using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Films;

public class CreateModel : PageModel
{
    private readonly IFilmService _filmService;

    public CreateModel(IFilmService filmService)
    {
        _filmService = filmService;
    }

    [BindProperty]
    public Film Film { get; set; } = new Film();

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

        await _filmService.CreateAsync(Film);

        return RedirectToPage("./Index");
    }
}
