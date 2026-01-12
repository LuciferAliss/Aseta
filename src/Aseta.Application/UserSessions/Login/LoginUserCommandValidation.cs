using System;
using Aseta.Domain.Entities.Users;
using FluentValidation;

namespace Aseta.Application.UserSessions.Login;

internal sealed class LoginUserCommandValidation : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidation()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email is required.");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("Password is required.");

        RuleFor(x => x.DeviceId)
            .NotNull().WithMessage("DeviceId is required.");

        RuleFor(x => x.DeviceName)
            .NotNull().WithMessage("DeviceName is required.");
    }
}
