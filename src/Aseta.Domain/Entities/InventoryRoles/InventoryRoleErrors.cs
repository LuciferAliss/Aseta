using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.InventoryRoles;

public static class InventoryRoleErrors
{
    public static Error InvalidRole() => Error.Validation(
        "InventoryRoles.InvalidRole",
        "The provided role is invalid.");

    public static Error UserAlreadyRole(Guid userId, Guid inventoryId, string role) => Error.Conflict(
        "Inventories.UserAlreadyRole",
        $"User with id: {userId} is already a {role} of inventory with id: {inventoryId}."
    );

    public static Error UserHasNoRole(Guid userId, Guid inventoryId) => Error.Conflict(
        "Inventories.UserHasNoRole",
        $"User with id: {userId} has no role in inventory with id: {inventoryId}."
    );
}
