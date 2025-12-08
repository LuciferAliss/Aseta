using System;
using Aseta.Domain.Entities.Users;
using FluentValidation;

namespace Aseta.Application.Users.Register;

internal sealed class RegisterUserCommandValidation : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidation()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.")
            .MaximumLength(User.MaxEmailLength).WithMessage($"Email must be at most {User.MaxEmailLength} characters long.");

        RuleFor(x => x.UserName)
            .NotNull().WithMessage("Username is required.")
            .MinimumLength(User.MinUserNameLength).WithMessage($"Username must be at least {User.MinUserNameLength} characters long.")
            .MaximumLength(User.MaxUserNameLength).WithMessage($"Username must be at most {User.MaxUserNameLength} characters long.");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("Password is required.")
            .MinimumLength(User.MinPasswordLength).WithMessage($"Password must be at least {User.MinPasswordLength} characters long.")
            .MaximumLength(User.MaxPasswordLength).WithMessage($"Password must be at most {User.MaxPasswordLength} characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
    }
}
