using Aseta.Domain.Entities.Inventories.CustomField;

namespace Aseta.Application.CustomFields.Delete;

public sealed record CustomFieldData(Guid FieldId, string Name, CustomFieldType Type);
