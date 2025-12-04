using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.Tags;

public static class TagErrors
{
    public static Error NameEmpty() => Error.Validation(
        "Tags.NameEmpty",
        "Tag name cannot be empty.");

    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Tags.NameTooLong",
        $"Tag name cannot be longer than {maxLength} characters.");
}
