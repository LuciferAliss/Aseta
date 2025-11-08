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

    public static Error NotPermission(Guid empty) => Error.Forbidden(
        "Users.NotPermission",
        "");
}