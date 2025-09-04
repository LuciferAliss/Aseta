using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Inventories;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class InventoryRepository(AppDbContext context) : Repository<Inventory, Guid>(context), IInventoryRepository
{
    public Task DeleteByFieldIdsAsync(List<Guid> deletedFieldIds)
    {
        return _dbSet.Where(f => deletedFieldIds.Contains(f.Id)).ExecuteDeleteAsync();
    }

    public async Task<List<Inventory>> GetAllPublicInventoriesAsync(Guid userId)
    {
        return await _dbSet
            .Include(i => i.Category)
            .Include(i => i.Tags)
            .Include(i => i.Creator)
            .Where(i => i.IsPublic || i.CreatorId == userId).ToListAsync();
    }
}