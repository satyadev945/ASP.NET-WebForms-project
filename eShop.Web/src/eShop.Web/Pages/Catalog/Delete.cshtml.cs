using eShop.Domain.Entities;
using eShop.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eShop.Web.Pages.Catalog;

public class DeleteModel : PageModel
{
    private readonly ICatalogService _catalogService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ICatalogService catalogService, ILogger<DeleteModel> logger)
    {
        _catalogService = catalogService;
        _logger = logger;
    }

    [BindProperty]
    public CatalogItem? Item { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Item = await _catalogService.GetCatalogItemByIdAsync(id);

            if (Item == null)
            {
                _logger.LogWarning("Catalog item not found for delete: {Id}", id);
                return NotFound();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading delete page for item: {Id}", id);
            return RedirectToPage("/Error");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Item?.Id == null)
        {
            return NotFound();
        }

        try
        {
            await _catalogService.DeleteCatalogItemAsync(Item.Id);
            _logger.LogInformation("Catalog item deleted: {Id}", Item.Id);

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting catalog item: {Id}", Item.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the item.");
            return Page();
        }
    }
}
