using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Films;

public class EditModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IFilmService filmService, ILogger<EditModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Името е задължително")]
        [StringLength(200, ErrorMessage = "Името трябва да е до 200 символа")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Описанието трябва да е до 1000 символа")]
        public string? Description { get; set; }

        [Range(1800, 2100, ErrorMessage = "Годината трябва да е между 1800 и 2100")]
        public int? Year { get; set; }

        public bool IsActive { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var film = await _filmService.GetByIdAsync(Id);

            if (film == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Name = film.Name,
                Description = film.Description,
                Year = film.Year,
                IsActive = film.IsActive
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film for editing with id {Id}", Id);
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var dto = new FilmUpdateDto
            {
                Name = Input.Name,
                Description = Input.Description,
                Year = Input.Year,
                IsActive = Input.IsActive
            };

            await _filmService.UpdateAsync(Id, dto);

            TempData["SuccessMessage"] = "Филмът е обновен успешно";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film with id {Id}", Id);
            ModelState.AddModelError(string.Empty, "Възникна грешка при обновяването на филма");
            return Page();
        }
    }
}
