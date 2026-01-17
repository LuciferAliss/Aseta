using System;
using FluentValidation;

namespace Aseta.Application.InventoryRoles.GetUser;

internal sealed class GetInventoryUserQueryValidation : AbstractValidator<GetInventoryUserQuery>
{
    public GetInventoryUserQueryValidation()
    {
        RuleFor(i => i.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("A valid InventoryId is required.");
    }
}
