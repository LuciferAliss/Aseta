using Aseta.Domain.Abstractions.Primitives;

namespace Aseta.Domain.Entities.Inventories.CustomField;

public class CustomFieldDefinition
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CustomFieldType Type { get; private set; }

    private CustomFieldDefinition(string name, CustomFieldType type)
    {
        Id = Guid.NewGuid();
        Name = name;
        Type = type;
    }

    public static CustomFieldDefinition Create(string name, CustomFieldType type) => new(name, type);
}