using Aseta.Domain.Abstractions.Primitives.Errors;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.Domain.Entities.Users;

public static class RefreshTokenErrors
{
    public static Error TokenEmpty() => Error.Validation(
        "RefreshToken.TokenEmpty",
        "Refresh token cannot be empty.");

    public static Error TokenTooLong(int length) => Error.Validation(
        "RefreshToken.TokenTooLong",
        $"Refresh token cannot be longer than {length} characters.");

    public static Error DeviceIdEmpty() => Error.Validation(
        "RefreshToken.DeviceIdEmpty",
        "Device ID cannot be empty.");

    public static Error DeviceIdTooLong(int length) => Error.Validation(
        "RefreshToken.DeviceIdTooLong",
        $"Device ID cannot be longer than {length} characters.");

    public static Error DeviceNameEmpty() => Error.Validation(
        "RefreshToken.DeviceNameEmpty",
        "Device name cannot be empty.");

    public static Error DeviceNameTooLong(int length) => Error.Validation(
        "RefreshToken.DeviceNameTooLong",
        $"Device name cannot be longer than {length} characters.");
}
