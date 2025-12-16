using System;
using Aseta.Domain.Entities.Inventories.CustomField;
using FluentValidation;

namespace Aseta.Application.Items.Update;

internal sealed class UpdateItemCommandValidation : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidation()
    {
        RuleFor(x => x.ItemId)
            .NotEqual(Guid.Empty).WithMessage("A valid ItemId is required.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("A valid CreatorId is required.");

        RuleFor(x => x.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("A valid InventoryId is required.");

        RuleForEach(x => x.CustomFieldValues)
            .ChildRules(cfv =>
            {
                cfv.RuleFor(x => x.FieldId).NotEqual(Guid.Empty).WithMessage("A valid FieldId is required.");

                cfv.RuleFor(x => x.Value).MaximumLength(CustomFieldValue.MaxValueLength)
                    .WithMessage($"The custom field value cannot exceed {CustomFieldValue.MaxValueLength} characters.");
            });

        RuleFor(x => x.CustomFieldValues)
            .Must(IsUniqueCustomFieldId).WithMessage("CustomFieldIds must be unique.");
    }

    private bool IsUniqueCustomFieldId(ICollection<CustomFieldValueData> items)
    {
        var seen = new HashSet<Guid>();
        foreach (CustomFieldValueData item in items)
        {
            if (!seen.Add(item.FieldId))
            {
                return false;
            }
        }
        return true;
    }
}
