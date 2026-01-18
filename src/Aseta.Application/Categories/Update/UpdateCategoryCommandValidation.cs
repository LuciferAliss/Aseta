using System;
using Aseta.Domain.Entities.Categories;
using FluentValidation;

namespace Aseta.Application.Categories.Update;

internal sealed class UpdateCategoryCommandValidation : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidation()
    {
        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty)
            .WithMessage("A valid CategoryId is required.");

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
