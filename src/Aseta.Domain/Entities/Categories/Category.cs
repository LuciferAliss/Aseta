using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Entities.Categories;

public class Category : Entity
{
    public string CategoryName { get; private set; }
    public virtual ICollection<Inventory> Inventories { get; private set; } = [];

    private Category(Guid id, string name) : base(id)
    {
        CategoryName = name;
    }

    public static Category Create(string name) => new(Guid.NewGuid(), name);
}