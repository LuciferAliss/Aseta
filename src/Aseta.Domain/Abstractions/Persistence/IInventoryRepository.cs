using Aseta.Domain.DTO.Inventories;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Abstractions.Persistence;

public interface IInventoryRepository : IRepository<Inventory>
{
    Task<(ICollection<Inventory> inventories, string? nextCursor, bool hasNextPage)> GetPaginatedWithKeysetAsync(
        InventoryPaginationParameters parameters,
        CancellationToken cancellationToken);
}