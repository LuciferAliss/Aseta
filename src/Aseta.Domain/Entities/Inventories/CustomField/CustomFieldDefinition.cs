using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.Domain.Entities.Inventories.CustomField;

public class CustomFieldDefinition
{
    public const int MaxNameLength = 50;

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CustomFieldType Type { get; private set; }

    private CustomFieldDefinition(string name, CustomFieldType type)
    {
        Id = Guid.NewGuid();
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

        return new CustomFieldDefinition(name, type);
    }
}