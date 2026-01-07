using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.FilmPages;

public class CreateModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IFilmService filmService, ILogger<CreateModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public FilmCreateViewModel Film { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var filmDto = new FilmCreateDto
            {
                Name = Film.Name,
                Year = Film.Year,
                Description = Film.Description
            };

            await _filmService.CreateAsync(filmDto);
            TempData["SuccessMessage"] = "Film created successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the film.");
            return Page();
        }
    }

    public class FilmCreateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year is required")]
        [Range(1888, 2100, ErrorMessage = "Year must be between 1888 and 2100")]
        public int Year { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }
    }
}
