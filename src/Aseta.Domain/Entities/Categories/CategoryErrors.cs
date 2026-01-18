using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.Categories;

public static class CategoryErrors
{
    public static Error NameEmpty() => Error.Validation(
        "Categories.NameEmpty",
        "Category name cannot be empty.");

    public static Error NotFound(Guid categoryId) => Error.NotFound(
        "Categories.NotFound",
        $"The category with id: {categoryId} was not found.");

    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Categories.NameTooLong",
        $"Category name cannot be longer than {maxLength} characters.");

    public static Error NameTooShort(int minLength) => Error.Validation(
        "Categories.NameTooShort",
        $"Category name cannot be shorter than {minLength} characters.");

    public static Error AlreadyExists(string name) => Error.Conflict(
        "Categories.AlreadyExists",
        $"Category with name {name} already exists.");

    public static Error DeletionFailed() => Error.Problem(
        "Categories.DeletionFailed",
        "The category deletion operation failed.");
}
