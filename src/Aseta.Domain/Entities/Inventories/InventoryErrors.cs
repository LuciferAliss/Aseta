using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.Inventories;

public static class InventoryErrors
{
    public static Error ImageUrlNull() => Error.Validation(
        "Inventories.ImageUrlNull",
        "Image url cannot be null.");

    public static Error NotFound(Guid inventoryId) => Error.NotFound(
        "Inventories.NotFound",
        $"The inventory with id: {inventoryId} was not found.");

    public static Error NameEmpty() => Error.Validation(
        "Inventories.NameEmpty",
        "Inventory name cannot be empty.");

    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Inventories.NameTooLong",
        $"Inventory name cannot be longer than {maxLength} characters.");

    public static Error NameTooShort(int minLength) => Error.Validation(
        "Inventories.NameTooShort",
        $"Inventory name cannot be shorter than {minLength} characters.");

    public static Error DescriptionTooLong(int maxLength) => Error.Validation(
        "Inventories.DescriptionTooLong",
        $"Description cannot be longer than {maxLength} characters.");

    public static Error CustomFieldLimitExceeded(int max, string type, int count) => Error.Validation(
        "Inventories.CustomFieldLimitExceeded",
        $"Cannot have more than {max} custom fields of type '{type}'. Found {count}.");

    public static Error CategoryIdEmpty() => Error.Validation(
        "Inventories.CategoryIdEmpty",
        "Category id cannot be empty."
    );
}
