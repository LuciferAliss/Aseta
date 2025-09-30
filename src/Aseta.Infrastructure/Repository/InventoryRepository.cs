using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Inventories;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class InventoryRepository(AppDbContext context) 
: Repository<Inventory>(context), IInventoryRepository
{
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }

    public async Task<ICollection<Inventory>> GetLastInventoriesPageAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Include(i => i.Creator)
            .OrderByDescending(i => i.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteByFieldIdsAsync(
        ICollection<Guid> deletedFieldIds,
        CancellationToken cancellationToken = default)
    {
        await _dbSet.Where(f => deletedFieldIds.Contains(f.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<ICollection<Inventory>> GetMostPopularInventoriesAsync(
        int itemCount,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Include(i => i.Creator)
            .OrderByDescending(i => i.Items.Count)
            .Take(itemCount)
            .ToListAsync(cancellationToken);
    }

    public async Task<Inventory?> GetByIdWithTagsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Include(i => i.Tags)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(i => i.Id == id, cancellationToken);
    }
}