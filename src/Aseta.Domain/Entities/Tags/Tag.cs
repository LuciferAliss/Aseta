using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Entities.Tags;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual List<Inventory> Inventories { get; private set; } = [];
}
