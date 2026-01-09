using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Directors;

public class IndexModel : PageModel
{
    private readonly IDirectorService _directorService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IDirectorService directorService, ILogger<IndexModel> logger)
    {
        _directorService = directorService;
        _logger = logger;
    }

    public IEnumerable<DirectorDto> Directors { get; set; } = new List<DirectorDto>();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Directors = await _directorService.GetAllAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading directors");
            ErrorMessage = "An error occurred while loading directors.";
            return Page();
        }
    }
}
