using Aseta.Domain.Entities.Inventories;
using FluentValidation;

namespace Aseta.Application.Inventories.Update;

internal sealed class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
{
    public UpdateInventoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(Inventory.MinNameLength)
            .MaximumLength(Inventory.MaxNameLength);

        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty()
            .MaximumLength(Inventory.MaxDescriptionLength);
    }
}