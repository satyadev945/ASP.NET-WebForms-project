using Films.Application.Interfaces;
using Films.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Films;

public class CreateModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IFilmService filmService, ILogger<CreateModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    [BindProperty]
    public FilmInputModel Film { get; set; } = new();

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

        try
        {
            var film = new Film
            {
                Title = Film.Title,
                Description = Film.Description,
                Year = Film.Year,
                Genre = Film.Genre
            };

            await _filmService.CreateFilmAsync(film);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the film.");
            return Page();
        }
    }

    public class FilmInputModel
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(1800, 2100)]
        public int? Year { get; set; }

        [StringLength(100)]
        public string? Genre { get; set; }
    }
}
