using Aseta.Domain.Abstractions;
using Aseta.Domain.Entities.CustomField;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Items;

public class Item
{
    public Guid Id { get; set; }
    public string CustomId { get; set; } = string.Empty;

    public Guid InventoryId { get; set; }
    public virtual Inventory Inventory { get; set; }

    public List<CustomFieldValue> CustomFieldValues { get; set; } = [];

    public DateTime CreatedAt { get; set; }

    public Guid CreatorId { get; set; }
    public virtual UserApplication Creator { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid UpdaterId { get; set; }
    public virtual UserApplication Updater { get; set; }
}
