using System.Text.Json.Serialization;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.Domain.Entities.Inventories.CustomField;

public class CustomFieldDefinition
{
    public const int MaxNameLength = 50;

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CustomFieldType Type { get; private set; }

    private CustomFieldDefinition() { }

    [JsonConstructor]
    private CustomFieldDefinition(Guid id, string name, CustomFieldType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public static Result<CustomFieldDefinition> Create(string name, CustomFieldType type)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return CustomFieldErrors.DefinitionNameEmpty();
        }

        if (name.Length > MaxNameLength)
        {
            return CustomFieldErrors.DefinitionNameTooLong(MaxNameLength);
        }

        if (type == CustomFieldType.None)
        {
            return CustomFieldErrors.InvalidType();
        }

        return new CustomFieldDefinition(Guid.NewGuid(), name, type);
    }

    public static Result<CustomFieldDefinition> Reconstitute(Guid id, string name, CustomFieldType type)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return CustomFieldErrors.DefinitionNameEmpty();
        }

        if (name.Length > MaxNameLength)
        {
            return CustomFieldErrors.DefinitionNameTooLong(MaxNameLength);
        }

        if (type == CustomFieldType.None)
        {
            return CustomFieldErrors.InvalidType();
        }

        if (id == Guid.Empty)
        {
            return CustomFieldErrors.IdEmpty();
        }

        return new CustomFieldDefinition(id, name, type);
    }
}