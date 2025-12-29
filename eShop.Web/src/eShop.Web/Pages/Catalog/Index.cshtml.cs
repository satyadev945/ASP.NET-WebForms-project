using eShop.Domain.Entities;
using eShop.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.Web.Pages.Catalog;

public class IndexModel : PageModel
{
    private readonly ICatalogService _catalogService;
    private readonly ILogger<IndexModel> _logger;
    private const int PageSize = 10;

    public IndexModel(ICatalogService catalogService, ILogger<IndexModel> logger)
    {
        _catalogService = catalogService;
        _logger = logger;
    }

    public IEnumerable<CatalogItem> Items { get; set; } = new List<CatalogItem>();
    public int PageIndex { get; set; }
    public long TotalCount { get; set; }
    public int TotalPages { get; set; }

    public async Task<IActionResult> OnGetAsync(int? pageIndex)
    {
        PageIndex = pageIndex ?? 0;

        try
        {
            var result = await _catalogService.GetCatalogItemsPaginatedAsync(PageSize, PageIndex);
            Items = result.Items;
            TotalCount = result.TotalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            _logger.LogInformation("Catalog page loaded - Page: {PageIndex}, Items: {Count}", PageIndex, Items.Count());
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading catalog page");
            return RedirectToPage("/Error");
        }
    }
}
