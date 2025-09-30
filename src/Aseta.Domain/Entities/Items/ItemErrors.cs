using Aseta.Domain.Abstractions;

namespace Aseta.Domain.Entities.Items;

public class ItemErrors
{
    public static Error NotFound(Guid itemId) => Error.NotFound(
        "Items.NotFound",
        $"The item with id: {itemId} was not found.");

    public static Error CustomIdIsRequired => Error.Conflict(
        "Items.CustomIdIsRequired",
         "Custom id is required.");
}
