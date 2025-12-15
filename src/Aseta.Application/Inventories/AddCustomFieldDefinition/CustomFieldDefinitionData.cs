using Aseta.Domain.Entities.Inventories.CustomField;

namespace Aseta.Application.Inventories.AddCustomFieldDefinition;

public record CustomFieldDefinitionData(string Name, CustomFieldType Type);
