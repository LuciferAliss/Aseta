using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.Domain.Entities.Inventories.CustomField;

public class CustomFieldValue
{
    public const int MaxValueLength = 1000;

    public Guid FieldId { get; private set; }
    public string? Value { get; private set; }

    private CustomFieldValue() { }

    private CustomFieldValue(Guid fieldId, string? value)
    {
        FieldId = fieldId;
        Value = value;
    }

    public static Result<CustomFieldValue> Create(Guid fieldId, string? value)
    {
        if (value is not null && value.Length > MaxValueLength)
        {
            return CustomFieldErrors.ValueTooLong(MaxValueLength);
        }

        return new CustomFieldValue(fieldId, value);
    }
}