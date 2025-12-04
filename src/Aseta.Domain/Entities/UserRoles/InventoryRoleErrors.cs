using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.UserRoles;

public static class InventoryRoleErrors
{
    public static Error InvalidRole() => Error.Validation(
        "InventoryRoles.InvalidRole",
        "The provided role is invalid.");
}
