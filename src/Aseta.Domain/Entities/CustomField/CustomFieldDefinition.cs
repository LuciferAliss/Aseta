using System.Text.Json.Serialization;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Entities.CustomField;

public class CustomFieldDefinition
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public CustomFieldType Type { get; private set; }
}