using System;
using Aseta.Domain.Entities.Inventories.CustomField;
using FluentValidation;

namespace Aseta.Application.CustomFields.Delete;

internal sealed class DeleteCustomFieldDefinitionCommandValidation : AbstractValidator<DeleteCustomFieldDefinitionCommand>
{
    public DeleteCustomFieldDefinitionCommandValidation()
    {
        RuleFor(x => x.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("InventoryId is required.");

        RuleFor(x => x.FieldId)
            .NotEqual(Guid.Empty).WithMessage("A valid FieldId is required.");
    }
}
