using System;
using Aseta.Domain.Entities.InventoryRoles;
using FluentValidation;

namespace Aseta.Application.InventoryRoles.Update;

internal sealed class UpdateRoleUserCommandValidation : AbstractValidator<UpdateRoleUserCommand>
{
    public UpdateRoleUserCommandValidation()
    {
        RuleFor(i => i.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("A valid InventoryId is required.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("A valid UserId is required.");

        RuleFor(x => x.Role)
            .NotEqual(Role.None).WithMessage("A valid Role is required.");
    }
}
