using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Films.Application.DTOs;
using Films.Application.Interfaces;

namespace Films.Web.Pages.FilmPages;

public class IndexModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IFilmService filmService, ILogger<IndexModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    public IEnumerable<FilmDto> Films { get; set; } = new List<FilmDto>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [TempData]
    public string? SuccessMessage { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Films = await _filmService.SearchAsync(SearchTerm);
            }
            else
            {
                Films = await _filmService.GetAllAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading films");
            Films = new List<FilmDto>();
        }
    }
}
