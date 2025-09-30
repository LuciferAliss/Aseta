namespace Aseta.Domain.Entities.Inventories;

public class Category(string name)
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public virtual ICollection<Inventory> Inventories { get; private set; } = [];
}