using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Items;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class ItemRepository(AppDbContext context) : Repository<Item, Guid>(context), IItemRepository
{
    public async Task<List<Item>> GetByItemsInventoryIdAsync(Guid inventoryId)
    {
        return await _dbSet.Where(i => i.InventoryId == inventoryId).ToListAsync();
    }

    public async Task<int> LastItemPosition(Guid inventoryId)
    {
        return await _dbSet.CountAsync(i => i.InventoryId == inventoryId);
    }
}