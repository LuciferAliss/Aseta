using Aseta.Domain.Abstractions.Primitives.Errors;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.Domain.Entities.Users;

public static class UserSessionErrors
{
    public static Error InvalidId(string id) => Error.Validation(
        "UserSession.InvalidId",
        $"User session id: {id} is invalid.");

    public static Error NotFound() => Error.NotFound(
        "UserSession.NotFound",
        "User session not found.");

    public static Error TokenIsInactive() => Error.Unauthorized(
        "UserSession.TokenIsInactive",
        "User session token is inactive.");

    public static Error SuspiciousActivity() => Error.Forbidden(
        "UserSession.SuspiciousActivity",
        "Suspicious activity detected. All user sessions have been terminated for security.");

    public static Error TokenEmpty() => Error.Validation(
        "UserSession.TokenEmpty",
        "Refresh token cannot be empty.");

    public static Error TokenTooLong(int length) => Error.Validation(
        "UserSession.TokenTooLong",
        $"Refresh token cannot be longer than {length} characters.");

    public static Error DeviceIdEmpty() => Error.Validation(
        "UserSession.DeviceIdEmpty",
        "Device ID cannot be empty.");

    public static Error DeviceIdTooLong(int length) => Error.Validation(
        "UserSession.DeviceIdTooLong",
        $"Device ID cannot be longer than {length} characters.");

    public static Error DeviceNameEmpty() => Error.Validation(
        "UserSession.DeviceNameEmpty",
        "Device name cannot be empty.");

    public static Error DeviceNameTooLong(int length) => Error.Validation(
        "UserSession.DeviceNameTooLong",
        $"Device name cannot be longer than {length} characters.");
}
