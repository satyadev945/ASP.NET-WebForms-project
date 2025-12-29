using eShop.Domain.Entities;
using eShop.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace eShop.Web.Pages.Catalog;

public class EditModel : PageModel
{
    private readonly ICatalogService _catalogService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ICatalogService catalogService, ILogger<EditModel> logger)
    {
        _catalogService = catalogService;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public SelectList CatalogTypes { get; set; } = new SelectList(new List<CatalogType>());
    public SelectList CatalogBrands { get; set; } = new SelectList(new List<CatalogBrand>());

    public class InputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 9999999999999999.99)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Type")]
        public int CatalogTypeId { get; set; }

        [Required]
        [Display(Name = "Brand")]
        public int CatalogBrandId { get; set; }

        [Required]
        [Range(0, 10000000)]
        public int AvailableStock { get; set; }

        [Required]
        [Display(Name = "Picture File Name")]
        public string PictureFileName { get; set; } = "dummy.png";
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var item = await _catalogService.GetCatalogItemByIdAsync(id);

            if (item == null)
            {
                _logger.LogWarning("Catalog item not found for edit: {Id}", id);
                return NotFound();
            }

            Input = new InputModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                CatalogTypeId = item.CatalogTypeId,
                CatalogBrandId = item.CatalogBrandId,
                AvailableStock = item.AvailableStock,
                PictureFileName = item.PictureFileName
            };

            var types = await _catalogService.GetCatalogTypesAsync();
            var brands = await _catalogService.GetCatalogBrandsAsync();

            CatalogTypes = new SelectList(types, "Id", "Type");
            CatalogBrands = new SelectList(brands, "Id", "Brand");

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading edit page for item: {Id}", id);
            return RedirectToPage("/Error");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var types = await _catalogService.GetCatalogTypesAsync();
            var brands = await _catalogService.GetCatalogBrandsAsync();
            CatalogTypes = new SelectList(types, "Id", "Type");
            CatalogBrands = new SelectList(brands, "Id", "Brand");
            return Page();
        }

        try
        {
            var item = await _catalogService.GetCatalogItemByIdAsync(Input.Id);

            if (item == null)
            {
                return NotFound();
            }

            item.Name = Input.Name;
            item.Description = Input.Description;
            item.Price = Input.Price;
            item.CatalogTypeId = Input.CatalogTypeId;
            item.CatalogBrandId = Input.CatalogBrandId;
            item.AvailableStock = Input.AvailableStock;
            item.PictureFileName = Input.PictureFileName;

            await _catalogService.UpdateCatalogItemAsync(item);
            _logger.LogInformation("Catalog item updated: {Id}", Input.Id);

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating catalog item: {Id}", Input.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the item.");

            var types = await _catalogService.GetCatalogTypesAsync();
            var brands = await _catalogService.GetCatalogBrandsAsync();
            CatalogTypes = new SelectList(types, "Id", "Type");
            CatalogBrands = new SelectList(brands, "Id", "Brand");

            return Page();
        }
    }
}
