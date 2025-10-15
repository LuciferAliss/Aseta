using Aseta.Domain.Abstractions.Persistence;

namespace Aseta.Domain.Entities.CustomId;

public class GenerationContext(IItemRepository itemRepository, Guid inventoryId)
{
    public Guid InventoryId { get; } = inventoryId;
    public IItemRepository ItemRepository { get; } = itemRepository;
}
