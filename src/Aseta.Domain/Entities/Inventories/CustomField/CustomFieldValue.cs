using Aseta.Domain.Abstractions.Primitives;

namespace Aseta.Domain.Entities.CustomField;

public class CustomFieldValue
{
    public Guid FieldId { get; private set; }
    public string? Value { get; private set; }

    private CustomFieldValue() { }

    private CustomFieldValue(Guid fieldId, string? value)
    {
        FieldId = fieldId;
        Value = value;
    }

    public static Result<CustomFieldValue> Create(Guid fieldId, string? value) => new CustomFieldValue(fieldId, value);
}