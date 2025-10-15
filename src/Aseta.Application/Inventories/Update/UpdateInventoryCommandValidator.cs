using FluentValidation;

namespace Aseta.Application.Inventories.Update;

internal sealed class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
{
    public UpdateInventoryCommandValidator()
    {
        RuleFor(x => x.InventoryId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.ImageUrl).NotEmpty();
        RuleFor(x => x.IsPublic).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}