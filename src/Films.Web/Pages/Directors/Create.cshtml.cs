using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Directors;

public class CreateModel : PageModel
{
    private readonly IDirectorService _directorService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IDirectorService directorService, ILogger<CreateModel> logger)
    {
        _directorService = directorService;
        _logger = logger;
    }

    [BindProperty]
    public DirectorInputModel Input { get; set; } = new();

    public class DirectorInputModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public int? SexId { get; set; }
    }

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
            var dto = new DirectorCreateDto
            {
                Name = Input.Name,
                Description = Input.Description,
                SexId = Input.SexId
            };

            await _directorService.CreateAsync(dto);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating director");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the director.");
            return Page();
        }
    }
}
