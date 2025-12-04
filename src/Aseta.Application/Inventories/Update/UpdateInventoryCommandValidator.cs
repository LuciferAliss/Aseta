using FluentValidation;

namespace Aseta.Application.Inventories.Update;

internal sealed class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
{
    public UpdateInventoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(36);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
    }
}