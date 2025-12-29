using eShop.Domain.Entities;
using eShop.Domain.Interfaces.Repositories;
using eShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eShop.Infrastructure.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly CatalogContext _context;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(CatalogContext context, ILogger<CatalogItemRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<CatalogItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.CatalogItems
                .Include(c => c.CatalogBrand)
                .Include(c => c.CatalogType)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all catalog items");
            throw;
        }
    }

    public async Task<CatalogItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.CatalogItems
                .Include(c => c.CatalogBrand)
                .Include(c => c.CatalogType)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving catalog item with ID: {Id}", id);
            throw;
        }
    }

    public async Task<(IEnumerable<CatalogItem> Items, long TotalCount)> GetPaginatedAsync(
        int pageSize, int pageIndex, CancellationToken cancellationToken = default)
    {
        try
        {
            var totalCount = await _context.CatalogItems.LongCountAsync(cancellationToken);

            var items = await _context.CatalogItems
                .Include(c => c.CatalogBrand)
                .Include(c => c.CatalogType)
                .OrderBy(c => c.Id)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paginated catalog items");
            throw;
        }
    }

    public async Task<CatalogItem> AddAsync(CatalogItem item, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.CatalogItems.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return item;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding catalog item: {Name}", item.Name);
            throw;
        }
    }

    public async Task UpdateAsync(CatalogItem item, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating catalog item: {Id}", item.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var item = await _context.CatalogItems.FindAsync(new object[] { id }, cancellationToken);
            if (item != null)
            {
                _context.CatalogItems.Remove(item);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting catalog item: {Id}", id);
            throw;
        }
    }
}
