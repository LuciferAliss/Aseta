using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Inventories;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class InventoryRepository(AppDbContext context) : Repository<Inventory, Guid>(context), IInventoryRepository
{
    public Task<int> CountPublicInventoriesAsync()
    {
        return _dbSet.CountAsync(i => i.IsPublic);
    }

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

    public Task<List<Inventory>> GetPublicInventoriesPageAsync(Guid userId, int pageNumber, int pageSize)
    {
        return _dbSet
            .Include(i => i.Category)
            .Include(i => i.Tags)
            .Include(i => i.Creator)
            .Where(i => i.IsPublic || i.CreatorId == userId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}