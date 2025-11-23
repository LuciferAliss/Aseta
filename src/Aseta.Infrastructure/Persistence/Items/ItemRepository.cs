using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Items;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Persistence.Items;

internal sealed class ItemRepository(AppDbContext context) : Repository<Item>(context), IItemRepository
{
    public async Task<int> GetItemSequenceNumberAsync(Guid itemId, Guid inventoryId, CancellationToken cancellationToken = default)
    {
        var item = await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == itemId && i.InventoryId == inventoryId, cancellationToken);

        if (item is null)
        {
            return 0; 
        }

        var sequence = await _dbSet.AsNoTracking()
            .Where(i => i.InventoryId == inventoryId && i.CreatedAt < item.CreatedAt)
            .CountAsync(cancellationToken);

        return sequence + 1;
    }
}