using System;
using FluentValidation;

namespace Aseta.Application.Inventories.Create;

internal sealed class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommand>
{
    public CreateInventoryCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.ImageUrl).NotEmpty();
        RuleFor(x => x.IsPublic).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
