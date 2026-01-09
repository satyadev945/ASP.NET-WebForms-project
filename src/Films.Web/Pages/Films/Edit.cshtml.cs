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
    public int Id { get; set; }

    [BindProperty]
    public FilmInputModel Input { get; set; } = new();

    public class FilmInputModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(1800, 2100)]
        public int? Year { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var film = await _filmService.GetByIdAsync(id);
            if (film == null)
            {
                return NotFound();
            }

            Id = film.Id;
            Input = new FilmInputModel
            {
                Name = film.Name,
                Year = film.Year,
                Description = film.Description
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film for edit, id {Id}", id);
            return RedirectToPage("Index");
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
                Year = Input.Year,
                Description = Input.Description
            };

            await _filmService.UpdateAsync(Id, dto);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film, id {Id}", Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the film.");
            return Page();
        }
    }
}
