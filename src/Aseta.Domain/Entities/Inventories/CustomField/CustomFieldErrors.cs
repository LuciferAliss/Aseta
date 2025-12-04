using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.Inventories.CustomField;

public static class CustomFieldErrors
{
    public static Error DefinitionNameEmpty() => Error.Validation(
        "CustomFields.DefinitionNameEmpty",
        "Custom field definition name cannot be empty.");

    public static Error DefinitionNameTooLong(int maxLength) => Error.Validation(
        "CustomFields.DefinitionNameTooLong",
        $"Custom field definition name cannot be longer than {maxLength} characters.");

    public static Error ValueTooLong(int maxLength) => Error.Validation(
        "CustomFields.ValueTooLong",
        $"Custom field value cannot be longer than {maxLength} characters.");
}
