using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Inventories;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class InventoryRepository(AppDbContext context) : Repository<Inventory, Guid>(context), IInventoryRepository
{
    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public override Task<Inventory?> GetByIdAsync(Guid id)
    {
        return _dbSet.Include(i => i.Creator).Include(i => i.Category).FirstOrDefaultAsync(i => i.Id == id);
    }

    public Task DeleteByFieldIdsAsync(List<Guid> deletedFieldIds)
    {
        return _dbSet.Where(f => deletedFieldIds.Contains(f.Id)).ExecuteDeleteAsync();
    }

    public async Task<List<Inventory>> GetLastInventoriesPageAsync(int pageNumber, int pageSize)
    {
        return await _dbSet.Include(i => i.Creator).OrderByDescending(i => i.CreatedAt).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public Task<int> CountPublicInventoriesAsync()
    {
        return _dbSet.CountAsync(i => i.IsPublic);
    }

    public async Task<List<Inventory>> GetMostPopularInventoriesAsync(int itemCount)
    {
        return await _dbSet.Include(i => i.Creator).OrderByDescending(i => i.Items.Count).Take(itemCount).ToListAsync();
    }

    public async Task<Inventory?> GetByIdWithTagsAsync(Guid id)
    {
        return await _dbSet.Include(i => i.Tags).FirstOrDefaultAsync(i => i.Id == id);
    }
}