using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.Inventories.CustomField;

public static class CustomFieldErrors
{
    public static Error DefinitionNameEmpty() => Error.Validation(
        "CustomFields.DefinitionNameEmpty",
        "Custom field definition name cannot be empty.");

    public static Error IdEmpty() => Error.Validation(
        "CustomFields.IdEmpty",
        "Custom field id cannot be empty.");
    public static Error DefinitionNameTooLong(int maxLength) => Error.Validation(
        "CustomFields.DefinitionNameTooLong",
        $"Custom field definition name cannot be longer than {maxLength} characters.");

    public static Error ValueTooLong(int maxLength) => Error.Validation(
        "CustomFields.ValueTooLong",
        $"Custom field value cannot be longer than {maxLength} characters.");

    public static Error NotFound(Guid customFieldId) => Error.NotFound(
        "CustomFields.NotFound",
        $"The custom field with id: {customFieldId} was not found.");

    public static Error NotFound(ICollection<Guid> customFieldIds) => Error.NotFound(
        "CustomFields.NotFound",
        $"The custom fields with ids: {string.Join(", ", customFieldIds)} were not found.");

    public static Error CannotUpdateNonExistentField(ICollection<Guid> customFieldIds) => Error.NotFound(
        "CustomFields.CannotUpdateNonExistentField",
        $"The custom fields with ids: {string.Join(", ", customFieldIds)} do not exist and cannot be updated.");

    public static Error CannotDeleteNonExistentField(ICollection<Guid> customFieldIds) => Error.NotFound(
        "CustomFields.CannotDeleteNonExistentField",
        $"The custom fields with ids: {string.Join(", ", customFieldIds)} do not exist and cannot be deleted.");

    public static Error InvalidValueForType(CustomFieldType type) => Error.Validation(
        "CustomFields.InvalidValueForType",
        $"The provided value is not valid for the custom field type '{type}'.");

    public static Error AllFieldsRequired() => Error.Validation(
        "CustomFields.AllFieldsRequired",
        "All defined custom fields must be provided.");

    public static Error InvalidType() => Error.Validation(
        "CustomFields.InvalidType",
        "The provided custom field type is invalid.");
}
