using System;
using FluentValidation;

namespace Aseta.Application.InventoryRoles.Delete;

internal sealed class DeleteRoleUserCommandValidation : AbstractValidator<DeleteRoleUserCommand>
{
    public DeleteRoleUserCommandValidation()
    {
        RuleFor(i => i.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("A valid InventoryId is required.");

        RuleFor(x => x.DeletedUserId)
            .NotEqual(Guid.Empty).WithMessage("A valid UserId is required.");
    }
}
