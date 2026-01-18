using System;
using FluentValidation;

namespace Aseta.Application.Tags.Delete;

internal sealed class DeleteTagsCommandValidation : AbstractValidator<DeleteTagsCommand>
{
    public DeleteTagsCommandValidation()
    {
        RuleForEach(x => x.TagIds)
            .NotEqual(Guid.Empty).WithMessage("A valid TagId is required.");
    }
}
