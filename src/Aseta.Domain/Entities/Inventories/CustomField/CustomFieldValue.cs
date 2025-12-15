using System.Globalization;
using System.Text.Json.Serialization;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.Domain.Entities.Inventories.CustomField;

public class CustomFieldValue
{
    public const int MaxValueLength = 1000;

    public Guid FieldId { get; private set; }
    public string? Value { get; private set; }

    private CustomFieldValue() { }

    [JsonConstructor]
    private CustomFieldValue(Guid fieldId, string? value)
    {
        FieldId = fieldId;
        Value = value;
    }

    public static Result<CustomFieldValue> Create(Guid fieldId, string? value, CustomFieldType type)
    {
        if (value is not null && value.Length > MaxValueLength)
        {
            return CustomFieldErrors.ValueTooLong(MaxValueLength);
        }

        Result validationResult = IsValueValidForType(type, value);
        if (validationResult.IsFailure)
        {
            return validationResult.Error;
        }

        return new CustomFieldValue(fieldId, value);
    }

    private static Result IsValueValidForType(CustomFieldType type, string? value)
    {
        if (value is null)
        {
            return Result.Success();
        }

        switch (type)
        {
            case CustomFieldType.Number:
                if (!decimal.TryParse(value, out _))
                {
                    return CustomFieldErrors.InvalidValueForType(type);
                }
                break;

            case CustomFieldType.Checkbox:
                if (!bool.TryParse(value, out _))
                {
                    return CustomFieldErrors.InvalidValueForType(type);
                }
                break;

            case CustomFieldType.Date:
                if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    return CustomFieldErrors.InvalidValueForType(type);
                }
                break;

            case CustomFieldType.SingleLineText:
            case CustomFieldType.MultiLineText:
                break;

            case CustomFieldType.None:
            default:
                break;
        }

        return Result.Success();
    }
}