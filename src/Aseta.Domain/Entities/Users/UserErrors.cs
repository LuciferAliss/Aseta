using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.Users;

public static class UserErrors
{
    public static Error InvalidId(string id) => Error.Validation(
        "Users.InvalidId",
        $"User id: {id} is invalid.");

    public static Error NotFound(string userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with id: {userId} was not found.");

    public static Error NotFoundByEmail(string email) => Error.NotFound(
        "Users.NotFoundByEmail",
        $"The user with email: {email} was not found.");

    public static Error NotAuthenticated() => Error.Unauthorized(
        "Users.NotAuthenticated",
        "User is not authenticated.");

    public static Error NotPermission(Guid inventoryId) => Error.Forbidden(
        "Users.NotPermission",
        $"User does not have required permission for inventory with id: {inventoryId}.");

    public static Error NotGlobalPermission(string role) => Error.Forbidden(
        "Users.NotGlobalPermission",
        $"User does not have required global role: {role}.");

    public static Error AccountLocked(string userId) => Error.Forbidden(
        "Users.AccountLocked",
        $"User account with id: {userId} is locked.");

    public static Error UserNameEmpty() => Error.Validation(
        "Users.UserNameEmpty",
        "User name cannot be empty.");

    public static Error EmailEmpty() => Error.Validation(
        "Users.EmailEmpty",
        "Email cannot be empty.");

    public static Error EmailInvalid() => Error.Validation(
        "Users.EmailInvalid",
        "Email is invalid.");

    public static Error UserAlreadyExists(string email, string userName) => Error.Conflict(
        "Users.UserAlreadyExists",
        $"User with email: {email} and user name: {userName} already exists.");

    public static Error PasswordEmpty() => Error.Validation(
        "Users.PasswordEmpty",
        "Password cannot be empty.");

    public static Error UserNameTooLong(int maxLength) => Error.Validation(
        "Users.UserNameTooLong",
        $"User name cannot be longer than {maxLength} characters.");

    public static Error UserNameTooShort(int minLength) => Error.Validation(
        "Users.UserNameTooShort",
        $"User name cannot be shorter than {minLength} characters.");

    public static Error EmailTooLong(int maxLength) => Error.Validation(
        "Users.EmailTooLong",
        $"Email cannot be longer than {maxLength} characters.");

    public static Error PasswordIncorrect() => Error.Validation(
        "Users.PasswordIncorrect",
        "Password is incorrect.");
}