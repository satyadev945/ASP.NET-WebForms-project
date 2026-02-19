using Films.Application.Interfaces;
using Films.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Films;

public class IndexModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IFilmService filmService, ILogger<IndexModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    public IEnumerable<Film> Films { get; set; } = new List<Film>();
    
    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            if (!string.IsNullOrEmpty(SearchString))
            {
                Films = await _filmService.SearchFilmsAsync(SearchString);
            }
            else
            {
                Films = await _filmService.GetAllFilmsAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading films");
            Films = new List<Film>();
        }
    }
}
