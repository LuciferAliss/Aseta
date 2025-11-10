using Aseta.Domain.Abstractions.Primitives;

namespace Aseta.Domain.Entities.Users;

public class UserErrors
{
    public static Error NotFound(string userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with id: {userId} was not found.");

    public static Error NotAuthenticated() => Error.Unauthorized(
        "Users.NotAuthenticated",
        "User is not authenticated.");

    public static Error NotPermission(Guid inventoryId) => Error.Forbidden(
        "Users.NotPermission",
        $"User does not have required permission for inventory with id: {inventoryId}.");

    public static Error AccountLocked(string userId) => Error.Forbidden(
        "Users.AccountLocked",
        $"User account with id: {userId} is locked.");
}