using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Services.CustomId;

internal static class CustomIdServiceErrors
{
    public static Error CustomIdEmpty() => Error.Problem(
        "CustomIdService.CustomIdEmpty",
        "CustomId is empty.");
    public static Error TemplateMismatch() => Error.Validation(
        "CustomIdService.TemplateMismatch",
        "The provided Id does not match the required template.");
}