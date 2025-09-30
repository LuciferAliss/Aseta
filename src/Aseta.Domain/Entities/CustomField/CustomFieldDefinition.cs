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
    public CustomFieldDefinition(Guid id, string name, CustomFieldType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public CustomFieldDefinition(string name, CustomFieldType type)
    {
        Id = Guid.NewGuid();
        Name = name;
        Type = type;
    }
}