using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Directors;

public class DetailsModel : PageModel
{
    private readonly IDirectorService _directorService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IDirectorService directorService, ILogger<DetailsModel> logger)
    {
        _directorService = directorService;
        _logger = logger;
    }

    public DirectorDto? Director { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Director = await _directorService.GetByIdAsync(id);
            if (Director == null)
            {
                return NotFound();
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading director details for id {Id}", id);
            return RedirectToPage("Index");
        }
    }
}
