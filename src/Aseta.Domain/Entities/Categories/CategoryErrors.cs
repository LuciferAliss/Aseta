using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Domain.Entities.Categories;

public static class CategoryErrors
{
    public static Error NameEmpty() => Error.Validation(
        "Categories.NameEmpty",
        "Category name cannot be empty.");

    public static Error NameTooLong(int maxLength) => Error.Validation(
        "Categories.NameTooLong",
        $"Category name cannot be longer than {maxLength} characters.");
}
