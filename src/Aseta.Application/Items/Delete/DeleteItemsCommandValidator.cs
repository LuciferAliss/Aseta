using System;
using FluentValidation;

namespace Aseta.Application.Items.Delete;

public class DeleteItemsCommandValidator
: AbstractValidator<DeleteItemsCommand>
{
    public DeleteItemsCommandValidator()
    {
        RuleFor(x => x.ItemIds).NotEmpty();
        RuleFor(x => x.InventoryId).NotEmpty();
    }
}
