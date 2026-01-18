using System;
using FluentValidation;

namespace Aseta.Application.Categories.Delete;

internal sealed class DeleteCategoriesCommandValidation : AbstractValidator<DeleteCategoriesCommand>
{
    public DeleteCategoriesCommandValidation()
    {
        RuleForEach(x => x.CategoryIds)
            .NotEqual(Guid.Empty).WithMessage("A valid CategoryId is required.");
    }
}
