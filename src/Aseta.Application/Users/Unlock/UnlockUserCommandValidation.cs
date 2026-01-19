using System;
using FluentValidation;

namespace Aseta.Application.Users.Unlock;

public class UnlockUserCommandValidation : AbstractValidator<UnlockUserCommand>
{
    public UnlockUserCommandValidation()
    {
        RuleFor(x => x.UserId).NotEqual(Guid.Empty).WithMessage("A valid UserId is required.");
    }
}
