using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Entities.Tags;

public class Tag : IEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public virtual ICollection<Inventory> Inventories { get; private set; } = [];
    
    private Tag()
    {
        Name = null!;
    }

    private Tag(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public static Result<Tag> Create(string name) => new Tag(name);
}
