using System;
using Aseta.Domain.Entities.Users;
using FluentValidation;

namespace Aseta.Application.UserSessions.Login;

internal sealed class LoginUserCommandValidation : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidation()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage(UserErrors.EmailEmpty().Description);

        RuleFor(x => x.Password)
            .NotNull().WithMessage(UserErrors.PasswordEmpty().Description);

        RuleFor(x => x.DeviceId)
            .NotNull().WithMessage(UserSessionErrors.DeviceIdEmpty().Description);

        RuleFor(x => x.DeviceName)
            .NotNull().WithMessage(UserSessionErrors.DeviceNameEmpty().Description);
    }
}
