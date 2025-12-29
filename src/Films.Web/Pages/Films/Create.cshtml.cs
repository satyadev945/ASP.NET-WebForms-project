using Films.Application.DTOs;
using Films.Application.Interfaces;
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
    public InputModel Input { get; set; } = default!;

    public class InputModel
    {
        [Required(ErrorMessage = "Името е задължително")]
        [StringLength(200, ErrorMessage = "Името трябва да е до 200 символа")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Описанието трябва да е до 1000 символа")]
        public string? Description { get; set; }

        [Range(1800, 2100, ErrorMessage = "Годината трябва да е между 1800 и 2100")]
        public int? Year { get; set; }
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var dto = new FilmCreateDto
            {
                Name = Input.Name,
                Description = Input.Description,
                Year = Input.Year
            };

            await _filmService.CreateAsync(dto);

            TempData["SuccessMessage"] = "Филмът е създаден успешно";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film");
            ModelState.AddModelError(string.Empty, "Възникна грешка при създаването на филма");
            return Page();
        }
    }
}
