using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.InventoryRoles;

public static class InventoryRoleErrors
{
    public static Error InvalidRole() => Error.Validation(
        "InventoryRoles.InvalidRole",
        "The provided role is invalid.");
}
