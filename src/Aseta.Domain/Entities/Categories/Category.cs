using System.ComponentModel;
using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Entities.Categories;

public class Category : Entity
{
    public const int MaxNameLength = 50;
    public const int MinNameLength = 3;

    public string Name { get; private set; }
    public virtual ICollection<Inventory> Inventories { get; private set; } = [];

    private Category(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static Result<Category> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return CategoryErrors.NameEmpty();
        }

        if (name.Length > MaxNameLength)
        {
            return CategoryErrors.NameTooLong(MaxNameLength);
        }

        if (name.Length < MinNameLength)
        {
            return CategoryErrors.NameTooShort(MinNameLength);
        }

        return new Category(Guid.NewGuid(), name);
    }

    public Result Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return CategoryErrors.NameEmpty();
        }

        if (name.Length > MaxNameLength)
        {
            return CategoryErrors.NameTooLong(MaxNameLength);
        }

        if (name.Length < MinNameLength)
        {
            return CategoryErrors.NameTooShort(MinNameLength);
        }

        Name = name;
        return Result.Success();
    }
}