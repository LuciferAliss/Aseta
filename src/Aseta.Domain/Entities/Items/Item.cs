using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Inventories.CustomField;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Items;

public class Item : Entity
{
    public string CustomId { get; private set; }
    public Guid InventoryId { get; private set; }
    public virtual Inventory Inventory { get; }
    public ICollection<CustomFieldValue> CustomFieldValues { get; private set; } = [];
    public Guid CreatorId { get; private set; }
    public virtual User Creator { get; }
    public Guid? UpdaterId { get; private set; }
    public virtual User Updater { get; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Item() { }

    private Item(Guid id, string customId, Guid inventoryId, ICollection<CustomFieldValue> customFieldValues, Guid creatorId) : base(id)
    {
        CustomId = customId;
        InventoryId = inventoryId;
        CustomFieldValues = customFieldValues;
        CreatedAt = DateTime.UtcNow;
        CreatorId = creatorId;
    }

    public static Result<Item> Create(string customId, Guid inventoryId, ICollection<CustomFieldValue> customFieldValues, Guid creatorId)
    {
        if (string.IsNullOrWhiteSpace(customId))
        {
            return ItemErrors.CustomIdIsRequired;
        }

        return new Item(Guid.NewGuid(), customId, inventoryId, customFieldValues, creatorId);
    }

    public Result Update(
        Guid updaterId,
        string newCustomId,
        ICollection<CustomFieldValue> newCustomFieldValues)
    {
        if (string.IsNullOrWhiteSpace(newCustomId))
        {
            return ItemErrors.CustomIdIsRequired;
        }

        UpdaterId = updaterId;
        UpdatedAt = DateTime.UtcNow;
        CustomId = newCustomId;
        CustomFieldValues = newCustomFieldValues;

        return Result.Success();
    }
}
