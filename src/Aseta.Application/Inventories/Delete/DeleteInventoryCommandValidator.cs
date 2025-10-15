using System;
using FluentValidation;

namespace Aseta.Application.Inventories.Delete;

internal sealed class DeleteInventoryCommandValidator : AbstractValidator<DeleteInventoryCommand>
{
    public DeleteInventoryCommandValidator()
    {
        RuleFor(x => x.InventoryId);
    }
}
