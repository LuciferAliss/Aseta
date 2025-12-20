using System;
using Aseta.Domain.Entities.Inventories.CustomField;
using FluentValidation;

namespace Aseta.Application.CustomFields.Update;

internal sealed class UpdateCustomFieldDefinitionCommandValidation : AbstractValidator<UpdateCustomFieldDefinitionCommand>
{
    public UpdateCustomFieldDefinitionCommandValidation()
    {
        RuleFor(x => x.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("InventoryId is required.");

        RuleForEach(x => x.CustomFields).ChildRules(cfv =>
        {
            cfv.RuleFor(x => x.Name)
                .MaximumLength(CustomFieldDefinition.MaxNameLength).WithMessage($"The custom field name cannot exceed {CustomFieldDefinition.MaxNameLength} characters.");

            cfv.RuleFor(x => x.Type)
                .NotEqual(CustomFieldType.None).WithMessage("A valid type is required.");

            cfv.RuleFor(x => x.FieldId)
                .NotEqual(Guid.Empty).WithMessage("A valid FieldId is required.");
        });

        RuleFor(x => x.CustomFields)
            .NotEmpty().WithMessage("The custom fields collection cannot be empty.");
    }
}
