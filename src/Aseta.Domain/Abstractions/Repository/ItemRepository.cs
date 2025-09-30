using Aseta.Domain.Entities.Items;

namespace Aseta.Domain.Abstractions.Repository;

public interface IItemRepository : IRepository<Item>
{
    Task<int> LastItemPosition(Guid inventoryId, CancellationToken cancellationToken = default);

    Task<ICollection<Item>> GetItemsPageAsync(
        Guid inventoryId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<int> CountItems(Guid inventoryId, CancellationToken cancellationToken = default);

    Task DeleteByItemIdsAsync(
        ICollection<Guid> itemIdsToRemove,
        Guid inventoryId,
        CancellationToken cancellationToken = default);
}
