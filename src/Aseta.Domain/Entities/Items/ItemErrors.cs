using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.Items;

public static class ItemErrors
{
    public static Error NotFound(Guid itemId) => Error.NotFound(
        "Items.NotFound",
        $"The item with id: {itemId} was not found.");

    public static Error CustomIdIsRequired => Error.Conflict(
        "Items.CustomIdIsRequired",
         "Custom id is required.");

    public static Error DeletionFailed() => Error.Problem(
        "Items.DeletionFailed",
        "The item deletion operation failed.");
}
