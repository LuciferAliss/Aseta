using Aseta.Domain.Entities.Inventories.CustomField;

namespace Aseta.Application.CustomFields.AddCustomFieldDefinition;

public record CustomFieldDefinitionData(string Name, CustomFieldType Type);
