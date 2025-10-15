using FluentValidation;

namespace Aseta.Application.Items.Delete;

internal sealed class DeleteItemsCommandValidator : AbstractValidator<DeleteItemsCommand>
{
    public DeleteItemsCommandValidator()
    {
        RuleFor(x => x.ItemIds).NotEmpty();
        RuleFor(x => x.InventoryId).NotEmpty();
    }
}
