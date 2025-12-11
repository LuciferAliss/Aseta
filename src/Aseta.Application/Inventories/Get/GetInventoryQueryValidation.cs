using System;
using FluentValidation;

namespace Aseta.Application.Inventories.Get;

internal sealed class GetInventoryQueryValidation : AbstractValidator<GetInventoryQuery>
{
    public GetInventoryQueryValidation()
    {
        RuleFor(i => i.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("A valid InventoryId is required.");
    }
}
