using FluentValidation;

namespace Aseta.Application.Items.Create;

internal sealed class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.InventoryId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}