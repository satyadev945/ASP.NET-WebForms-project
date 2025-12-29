using eShop.Domain.Entities;
using eShop.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.Web.Pages.Catalog;

public class DetailsModel : PageModel
{
    private readonly ICatalogService _catalogService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ICatalogService catalogService, ILogger<DetailsModel> logger)
    {
        _catalogService = catalogService;
        _logger = logger;
    }

    public CatalogItem? Item { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Item = await _catalogService.GetCatalogItemByIdAsync(id);

            if (Item == null)
            {
                _logger.LogWarning("Catalog item not found: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Catalog item details loaded: {Id}", id);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading catalog item details: {Id}", id);
            return RedirectToPage("/Error");
        }
    }
}
