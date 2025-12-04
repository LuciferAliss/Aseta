using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.Inventories;

public static class InventoryErrors
{
    public static Error NotFound(Guid inventoryId) => Error.NotFound(
        "Inventories.NotFound",
        $"The inventory with id: {inventoryId} was not found.");

    public static Error NameEmpty() => Error.Validation(
        "Inventories.NameEmpty",
        "Inventory name cannot be empty.");

    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Inventories.NameTooLong",
        $"Inventory name cannot be longer than {maxLength} characters.");

    public static Error DescriptionTooLong(int maxLength) => Error.Validation(
        "Inventories.DescriptionTooLong",
        $"Description cannot be longer than {maxLength} characters.");

    public static Error ImageUrlNull() => Error.Validation(
        "Inventories.ImageUrlNull",
        "Image URL cannot be null.");

    public static Error CustomFieldLimitExceeded(int max, string type, int count) => Error.Validation(
        "Inventories.CustomFieldLimitExceeded",
        $"Cannot have more than {max} custom fields of type '{type}'. Found {count}.");

    public static Error DescriptionEmpty() => Error.Validation(
        "Inventories.DescriptionEmpty",
        "Description cannot be empty.");
}
