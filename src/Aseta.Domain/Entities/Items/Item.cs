using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.CustomField;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Items;

public class Item
{
    public Guid Id { get; private set; }
    public string CustomId { get; private set; } = string.Empty;

    public Guid InventoryId { get; private set; }
    public virtual Inventory Inventory { get; private set; } = null!;

    public ICollection<CustomFieldValue> CustomFieldValues { get; private set; } = [];

    public DateTime CreatedAt { get; private set; }

    public Guid CreatorId { get; private set; }
    public virtual UserApplication Creator { get; private set; } = null!;

    public DateTime UpdatedAt { get; private set; }

    public Guid UpdaterId { get; private set; }
    public virtual UserApplication Updater { get; private set; } = null!;

    private Item() { }

    private Item(Guid id, string customId, Guid inventoryId, ICollection<CustomFieldValue> customFieldValues, Guid creatorId)
    {
        Id = id;
        CustomId = customId;
        InventoryId = inventoryId;
        CustomFieldValues = customFieldValues;
        CreatedAt = DateTime.UtcNow;
        CreatorId = creatorId;
    }

    public static Item Create(string customId, Guid inventoryId, ICollection<CustomFieldValue> customFieldValues, Guid creatorId) =>
        new(Guid.NewGuid(), customId, inventoryId, customFieldValues, creatorId);

    public Result Update(
        Guid updaterId,
        string newCustomId,
        ICollection<CustomFieldValue> newCustomFieldValues)
    {
        if (string.IsNullOrWhiteSpace(newCustomId))
        {
            return Result.Failure(ItemErrors.CustomIdIsRequired);
        }

        UpdaterId = updaterId;
        UpdatedAt = DateTime.UtcNow;
        CustomId = newCustomId;
        CustomFieldValues = newCustomFieldValues;

        return Result.Success();
    }
}
