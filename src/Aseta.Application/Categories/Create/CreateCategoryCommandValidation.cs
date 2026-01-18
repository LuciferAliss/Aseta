using System;
using Aseta.Domain.Entities.Categories;
using FluentValidation;

namespace Aseta.Application.Categories.Create;

internal sealed class CreateCategoryCommandValidation : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(CategoryErrors.NameEmpty().Description);

        RuleFor(x => x.Name)
            .MaximumLength(Category.MaxNameLength)
            .WithMessage(CategoryErrors.NameTooLong(Category.MaxNameLength).Description);

        RuleFor(x => x.Name)
            .MinimumLength(Category.MinNameLength)
            .WithMessage(CategoryErrors.NameTooShort(Category.MinNameLength).Description);
    }
}
