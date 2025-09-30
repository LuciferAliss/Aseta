using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Abstractions.Repository;

public interface IInventoryRepository : IRepository<Inventory>
{
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<ICollection<Inventory>> GetLastInventoriesPageAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task DeleteByFieldIdsAsync(
        ICollection<Guid> deletedFieldIds,
        CancellationToken cancellationToken = default);

    Task<ICollection<Inventory>> GetMostPopularInventoriesAsync(
        int itemCount,
        CancellationToken cancellationToken = default);

    Task<Inventory?> GetByIdWithTagsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}