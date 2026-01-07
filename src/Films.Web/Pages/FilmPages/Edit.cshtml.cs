using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.FilmPages;

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
    public FilmEditViewModel Film { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var filmDto = await _filmService.GetByIdAsync(id);
            if (filmDto == null)
            {
                return NotFound();
            }

            Film = new FilmEditViewModel
            {
                Id = filmDto.Id,
                Name = filmDto.Name,
                Year = filmDto.Year,
                Description = filmDto.Description
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film {FilmId}", id);
            return NotFound();
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
            var filmDto = new FilmUpdateDto
            {
                Name = Film.Name,
                Year = Film.Year,
                Description = Film.Description
            };

            await _filmService.UpdateAsync(Film.Id, filmDto);
            TempData["SuccessMessage"] = "Film updated successfully.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film {FilmId}", Film.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the film.");
            return Page();
        }
    }

    public class FilmEditViewModel
    {
        public int Id { get; set; }

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
