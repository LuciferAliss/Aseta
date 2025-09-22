namespace Aseta.Domain.Entities.Inventories;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual List<Inventory> Inventories { get; private set; } = [];

    public Category() {}

    public Category(string name)
    {
        Name = name;
    }
}