using System;
using Aseta.Domain.Entities.Users;
using FluentValidation;

namespace Aseta.Application.Users.GetByEmail;

internal sealed class GetByEmailQueryValidation : AbstractValidator<GetByEmailQuery>
{
    public GetByEmailQueryValidation()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage(UserErrors.EmailEmpty().Description)
            .EmailAddress().WithMessage(UserErrors.EmailInvalid().Description)
            .MaximumLength(User.MaxEmailLength).WithMessage(UserErrors.EmailTooLong(User.MaxEmailLength).Description);
    }
}
