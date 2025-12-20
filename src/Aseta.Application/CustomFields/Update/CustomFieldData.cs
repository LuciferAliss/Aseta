using Aseta.Domain.Entities.Inventories.CustomField;

namespace Aseta.Application.CustomFields.Update;

public sealed record CustomFieldData(Guid FieldId, string Name, CustomFieldType Type);
