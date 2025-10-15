using Aseta.Domain.Abstractions.Primitives;

namespace Aseta.Domain.Entities.Users;

public class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with id: {userId} was not found.");

    public static Error NotFound() => Error.NotFound(
        "Users.NotFound",
        $"The user was not found.");    

    public static Error NotPermission(Guid userId) => Error.Forbidden(
        "Users.NotPermission",
        $"The user with id: {userId} does not have permission to perform this action.");

    public static Error InvalidRole(Guid userId) => Error.Forbidden(
        "Users.InvalidRole",
        $"The user with id: {userId} does not have the required role to perform this action.");

    public static Error NotAuthenticated() => Error.Unauthorized(
        "Users.NotAuthenticated",
        "The user is not authenticated.");
}