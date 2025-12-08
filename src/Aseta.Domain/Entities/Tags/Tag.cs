using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Entities.Tags;

public class Tag : Entity
{
    public const int MaxNameLength = 25;

    public string Name { get; private set; }
    public virtual ICollection<Inventory> Inventories { get; }

    private Tag() { }

    private Tag(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static Result<Tag> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return TagErrors.NameEmpty();
        }

        if (name.Length > MaxNameLength)
        {
            return TagErrors.NameTooLong(MaxNameLength);
        }

        return new Tag(Guid.NewGuid(), name);
    }
}
