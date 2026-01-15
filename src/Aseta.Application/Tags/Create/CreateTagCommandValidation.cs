using System;
using Aseta.Domain.Entities.Tags;
using FluentValidation;

namespace Aseta.Application.Tags.Create;

internal sealed class CreateTagCommandValidation : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidation()
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
    }
}
