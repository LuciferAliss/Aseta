using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Inventories;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class InventoryRepository(AppDbContext context) : Repository<Inventory, Guid>(context), IInventoryRepository
{
    public async Task<List<Inventory>> GetAllPublicInventoriesAsync(Guid userId)
    {
        return await _dbSet.Where(i => i.IsPublic && i.CreatorId != userId).ToListAsync();
    }

    public async Task<bool> ItemContainsInventoryAsync(Guid inventoryId, Guid itemId)
    {
        return await _dbSet.AnyAsync(i => i.Id == inventoryId && i.Items.Any(ii => ii.Id == itemId));
    }
}