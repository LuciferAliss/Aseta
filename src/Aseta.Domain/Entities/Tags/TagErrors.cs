using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.Tags;

public static class TagErrors
{
    public static Error NameEmpty() => Error.Validation(
        "Tags.NameEmpty",
        "Tag name cannot be empty.");

    public static Error NotFound(Guid tagId) => Error.NotFound(
        "Tags.NotFound",
        $"The tag with id: {tagId} was not found."
    );

    public static Error NotFound() => Error.NotFound(
        "Tags.NotFound",
        "One or more tags were not found.");

    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Tags.NameTooLong",
        $"Tag name cannot be longer than {maxLength} characters.");

    public static Error NameTooShort(int minLength) => Error.Validation(
        "Tags.NameTooShort",
        $"Tag name cannot be shorter than {minLength} characters.");

    public static Error AlreadyExists(string name) => Error.Conflict(
        "Tags.AlreadyExists",
        $"Tag with name {name} already exists.");

    public static Error DeletionFailed() => Error.Problem(
        "Tags.DeletionFailed",
        "The tag deletion operation failed.");
}
