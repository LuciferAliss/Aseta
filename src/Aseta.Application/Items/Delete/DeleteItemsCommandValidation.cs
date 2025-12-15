using System;
using System.Data;
using FluentValidation;

namespace Aseta.Application.Items.Delete;

internal sealed class DeleteItemsCommandValidation : AbstractValidator<DeleteItemsCommand>
{
    public DeleteItemsCommandValidation()
    {
        RuleForEach(x => x.ItemIds)
            .NotEqual(Guid.Empty).WithMessage("A valid ItemId is required.");

        RuleFor(x => x.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("A valid InventoryId is required.");
    }
}
