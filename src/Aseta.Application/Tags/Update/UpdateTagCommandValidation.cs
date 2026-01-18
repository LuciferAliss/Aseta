using System;
using Aseta.Domain.Entities.Tags;
using FluentValidation;

namespace Aseta.Application.Tags.Update;

internal sealed class UpdateTagCommandValidation : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(TagErrors.NameEmpty().Description);

        RuleFor(x => x.Name)
            .MaximumLength(Tag.MaxNameLength)
            .WithMessage(TagErrors.NameTooLong(Tag.MaxNameLength).Description);

        RuleFor(x => x.Name)
            .MinimumLength(Tag.MinNameLength)
            .WithMessage(TagErrors.NameTooShort(Tag.MinNameLength).Description);

        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("A valid TagId is required.");
    }
}
