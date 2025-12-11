using System;
using FluentValidation;

namespace Aseta.Application.Inventories.Delete;

internal sealed class DeleteInventoryCommandValidation : AbstractValidator<DeleteInventoryCommand>
{
    public DeleteInventoryCommandValidation()
    {
        RuleFor(i => i.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("A valid InventoryId is required.");
    }
}
