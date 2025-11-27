using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Inventories.CustomField;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Items;

public class Item : Entity
{
    public string CustomId { get; private set; }
    public Guid InventoryId { get; private set; }
    public virtual Inventory Inventory { get; private set; }
    public ICollection<CustomFieldValue> CustomFieldValues { get; private set; } = [];
    public Guid CreatorId { get; private set; }
    public virtual ApplicationUser Creator { get; private set; }
    public Guid UpdaterId { get; private set; }
    public virtual ApplicationUser Updater { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Item()
    {
        CustomId = null!;
        Inventory = null!;
        Creator = null!;
        Updater = null!;
    }

    private Item(Guid id, string customId, Guid inventoryId, ICollection<CustomFieldValue> customFieldValues, Guid creatorId) : base(id)
    {
        CustomId = customId;
        InventoryId = inventoryId;
        CustomFieldValues = customFieldValues;
        CreatedAt = DateTime.UtcNow;
        CreatorId = creatorId;
        Inventory = null!;
        Creator = null!;
        Updater = null!;
    }

    public static Item Create(string customId, Guid inventoryId, ICollection<CustomFieldValue> customFieldValues, Guid creatorId) 
        => new(Guid.NewGuid(), customId, inventoryId, customFieldValues, creatorId);

    public void Update(
        Guid updaterId,
        string newCustomId,
        ICollection<CustomFieldValue> newCustomFieldValues)
    {
        if (string.IsNullOrWhiteSpace(newCustomId))
        {
            throw new ArgumentException("CustomId cannot be null or whitespace.", nameof(newCustomId));
        }

        UpdaterId = updaterId;
        UpdatedAt = DateTime.UtcNow;
        CustomId = newCustomId;
        CustomFieldValues = newCustomFieldValues;
    }
}
