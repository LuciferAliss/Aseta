using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Items;

public class Item
{
    public Guid Id { get; private set; }
    public string CustomId { get; private set; }

    public Guid InventoryId { get; private set; }
    public virtual Inventory Inventory { get; private set; }

    public List<CustomField> CustomFields { get; private set; } = [];

    public DateTime CreatedAt { get; private set; }

    public Guid CreatorId { get; private set; }
    public virtual UserApplication Creator { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public Guid UpdaterId { get; private set; }
    public virtual UserApplication Updater { get; private set; }

    private Item(Guid id,
        string customId,
        List<CustomField> customFields,
        Guid inventoryId,
        DateTime createdAt,
        Guid creatorId,
        DateTime updatedAt,
        Guid updaterId)
    {
        Id = id;
        CustomId = customId;
        CustomFields = customFields;
        InventoryId = inventoryId;
        CreatedAt = createdAt;
        CreatorId = creatorId;
        UpdatedAt = updatedAt;
        UpdaterId = updaterId;
    }

    private Item() { }

    public static Item Create(string customId,
        Guid inventoryId,
        Guid creatorId,
        List<CustomField> customFields)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customId, nameof(customId));

        return new Item(
            Guid.NewGuid(),
            customId,
            customFields,
            inventoryId,
            DateTime.UtcNow,
            creatorId,
            DateTime.UtcNow,
            creatorId
        );
    } 
}