using Aseta.Domain.DTO.CustomField;
using Aseta.Domain.Entities.CustomField;
using AutoMapper;

namespace Aseta.Application.Mapping;

public class CustomFieldValueResolver
    : IValueResolver<CustomFieldDefinition, CustomFieldValue, string?>
{
    public string? Resolve(
        CustomFieldDefinition source,
        CustomFieldValue destination,
        string? destMember,
        ResolutionContext context
    )
    {
        if (!context.Items.TryGetValue("requestedFields", out var requestedFieldsObject))
            return null;

        if (requestedFieldsObject is not ICollection<CustomFieldValueRequest> requestedFields)
            return null;

        var requestValue = requestedFields.FirstOrDefault(f => f.FieldId == source.Id);

        return requestValue?.Value;
    }
}