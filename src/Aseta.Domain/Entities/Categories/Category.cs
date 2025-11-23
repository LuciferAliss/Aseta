using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Entities.Categories;

public class Category : IEntity
{
    public Guid Id { get; private set; }
    public string CategoryName { get; private set; }
    public virtual ICollection<Inventory> Inventories { get; private set; } = [];

    private Category(string name)
    {
        Id = Guid.NewGuid();
        CategoryName = name;
    }

    public static Result<Category> Create(string name) => new Category(name);
}