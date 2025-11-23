using Aseta.Domain.Entities.Items;

namespace Aseta.Domain.Abstractions.Persistence;

public interface IItemRepository : IRepository<Item>
{
    Task<int> GetItemSequenceNumberAsync(Guid itemId, Guid inventoryId, CancellationToken cancellationToken = default);
}