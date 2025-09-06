using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Items;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class ItemRepository(AppDbContext context) : Repository<Item, Guid>(context), IItemRepository
{
    public async Task<int> CountItems(Guid inventoryId)
    {
        return await _dbSet.CountAsync(i => i.InventoryId == inventoryId);
    }

    public async Task<List<Item>> GetByItemsInventoryIdAsync(Guid inventoryId)
    {
        return await _dbSet.Where(i => i.InventoryId == inventoryId).ToListAsync();
    }

    public Task<List<Item>> GetItemsPageAsync(Guid inventoryId, int pageNumber, int pageSize)
    {
        return _dbSet
            .Where(i => i.InventoryId == inventoryId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> LastItemPosition(Guid inventoryId)
    {
        return await _dbSet.CountAsync(i => i.InventoryId == inventoryId);
    }
}