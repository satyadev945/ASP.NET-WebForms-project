using eShop.Domain.Entities;
using eShop.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace eShop.Web.Pages.Catalog;

public class CreateModel : PageModel
{
    private readonly ICatalogService _catalogService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(ICatalogService catalogService, ILogger<CreateModel> logger)
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

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var types = await _catalogService.GetCatalogTypesAsync();
            var brands = await _catalogService.GetCatalogBrandsAsync();

            CatalogTypes = new SelectList(types, "Id", "Type");
            CatalogBrands = new SelectList(brands, "Id", "Brand");

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create page");
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
            var item = new CatalogItem
            {
                Name = Input.Name,
                Description = Input.Description,
                Price = Input.Price,
                CatalogTypeId = Input.CatalogTypeId,
                CatalogBrandId = Input.CatalogBrandId,
                AvailableStock = Input.AvailableStock,
                PictureFileName = Input.PictureFileName
            };

            await _catalogService.CreateCatalogItemAsync(item);
            _logger.LogInformation("Catalog item created: {Name}", Input.Name);

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating catalog item");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the item.");

            var types = await _catalogService.GetCatalogTypesAsync();
            var brands = await _catalogService.GetCatalogBrandsAsync();
            CatalogTypes = new SelectList(types, "Id", "Type");
            CatalogBrands = new SelectList(brands, "Id", "Brand");

            return Page();
        }
    }
}
