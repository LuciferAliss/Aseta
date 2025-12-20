using Aseta.Domain.Entities.Inventories.CustomField;
using FluentValidation;

namespace Aseta.Application.CustomFields.Add;

internal sealed class AddCustomFieldDefinitionCommandValidation : AbstractValidator<AddCustomFieldDefinitionCommand>
{
    public AddCustomFieldDefinitionCommandValidation()
    {
        RuleFor(x => x.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("InventoryId is required.");

        RuleForEach(x => x.NewFields).ChildRules(cfv =>
        {
            cfv.RuleFor(x => x.Name)
                .MaximumLength(CustomFieldDefinition.MaxNameLength).WithMessage($"The custom field name cannot exceed {CustomFieldDefinition.MaxNameLength} characters.");

            cfv.RuleFor(x => x.Type)
                .NotEqual(CustomFieldType.None).WithMessage("A valid type is required.");
        });

        RuleFor(x => x.NewFields)
            .NotEmpty().WithMessage("The NewFields collection cannot be empty.");
    }
}
