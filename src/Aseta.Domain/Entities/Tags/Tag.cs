using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Entities.Tags;

public class Tag : Entity
{
    public string Name { get; private set; }
    public virtual ICollection<Inventory> Inventories { get; private set; } = [];
    
    private Tag()
    {
        Name = null!;
    }

    private Tag(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static Tag Create(string name) => new(Guid.NewGuid(), name);
}
