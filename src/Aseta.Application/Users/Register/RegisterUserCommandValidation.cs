using System;
using Aseta.Domain.Entities.Users;
using FluentValidation;

namespace Aseta.Application.Users.Register;

internal sealed class RegisterUserCommandValidation : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidation()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage(UserErrors.EmailEmpty().Description)
            .EmailAddress().WithMessage(UserErrors.EmailInvalid().Description)
            .MaximumLength(User.MaxEmailLength).WithMessage(UserErrors.EmailTooLong(User.MaxEmailLength).Description);

        RuleFor(x => x.UserName)
            .NotNull().WithMessage(UserErrors.UserNameEmpty().Description)
            .MinimumLength(User.MinUserNameLength).WithMessage(UserErrors.UserNameTooShort(User.MinUserNameLength).Description)
            .MaximumLength(User.MaxUserNameLength).WithMessage(UserErrors.UserNameTooLong(User.MaxUserNameLength).Description);

        RuleFor(x => x.Password)
            .NotNull().WithMessage(UserErrors.PasswordEmpty().Description)
            .MinimumLength(User.MinPasswordLength).WithMessage($"Password must be at least {User.MinPasswordLength} characters long.")
            .MaximumLength(User.MaxPasswordLength).WithMessage($"Password must be at most {User.MaxPasswordLength} characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
    }
}
