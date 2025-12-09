using Aseta.Domain.Entities.Inventories;
using FluentValidation;

namespace Aseta.Application.Inventories.Create;

internal sealed class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommand>
{
    public CreateInventoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name is required.")
            .MinimumLength(Inventory.MinNameLength).WithMessage($"Name must be at least {Inventory.MinNameLength} characters long.")
            .MaximumLength(Inventory.MaxNameLength).WithMessage($"Name must be at most {Inventory.MaxNameLength} characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(Inventory.MaxDescriptionLength).WithMessage($"Description must be at most {Inventory.MaxDescriptionLength} characters long.");

        RuleFor(x => x.ImageUrl)
            .NotNull().WithMessage("ImageUrl is required.");

        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty).WithMessage("A valid CategoryId is required.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("A valid CreatorId is required.");
    }
}