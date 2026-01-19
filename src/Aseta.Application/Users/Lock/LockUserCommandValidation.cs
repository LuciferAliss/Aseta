using System;
using FluentValidation;

namespace Aseta.Application.Users.Lock;

internal sealed class LockUserCommandValidation : AbstractValidator<LockUserCommand>
{
    public LockUserCommandValidation()
    {
        RuleFor(x => x.UserId).NotEqual(Guid.Empty).WithMessage("A valid UserId is required.");
    }
}
