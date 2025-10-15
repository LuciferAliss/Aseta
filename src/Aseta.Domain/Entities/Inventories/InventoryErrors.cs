using Aseta.Domain.Abstractions.Primitives;

namespace Aseta.Domain.Entities.Inventories;

public class InventoryErrors
{
    public static Error NotFound(Guid inventoryId) => Error.NotFound(
        "Inventories.NotFound",
        $"The inventory with id: {inventoryId} was not found.");
}
