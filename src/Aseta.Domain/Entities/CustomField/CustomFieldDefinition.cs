using System.Text.Json.Serialization;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Entities.CustomField;

public class CustomFieldDefinition
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CustomFieldType Type { get; set; }

    private CustomFieldDefinition() { }

    [JsonConstructor]
    private CustomFieldDefinition(Guid id, string name, CustomFieldType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public static CustomFieldDefinition Create(string name, CustomFieldType type)
    {
        return new CustomFieldDefinition
        (
            Guid.NewGuid(),
            name,
            type
        );
    }
    
    public static CustomFieldDefinition Create(Guid? id, string name, CustomFieldType type)
    {
        return new CustomFieldDefinition
        (
            id ?? Guid.NewGuid(),
            name,
            type
        );
    }
}