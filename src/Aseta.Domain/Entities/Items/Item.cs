using Aseta.Domain.Abstractions;
using Aseta.Domain.Entities.CustomField;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Items;

public class Item
{
    public Guid Id { get; private set; }
    public string CustomId { get; private set; }

    public Guid InventoryId { get; private set; }
    public virtual Inventory Inventory { get; private set; }

    public List<CustomFieldValue> CustomFieldValues { get; private set; } = [];

    public DateTime CreatedAt { get; private set; }

    public Guid CreatorId { get; private set; }
    public virtual UserApplication Creator { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public Guid UpdaterId { get; private set; }
    public virtual UserApplication Updater { get; private set; }

    private Item(Guid id,
        string customId,
        List<CustomFieldValue> customFieldValues,
        Guid inventoryId,
        DateTime createdAt,
        Guid creatorId,
        DateTime updatedAt,
        Guid updaterId)
    {
        Id = id;
        CustomId = customId;
        CustomFieldValues = customFieldValues;
        InventoryId = inventoryId;
        CreatedAt = createdAt;
        CreatorId = creatorId;
        UpdatedAt = updatedAt;
        UpdaterId = updaterId;
    }

    private Item() { }

    public static Result<Item> Create(
        string customId,
        Guid inventoryId,
        Guid creatorId,
        List<CustomFieldValue> customFieldValues
    )
    {
        if (inventoryId == Guid.Empty) return ItemErrors.InventoryIdEmpty;

        if (string.IsNullOrWhiteSpace(customId)) return ItemErrors.CustomIdEmpty;

        if (creatorId == Guid.Empty) return ItemErrors.CreatorIdEmpty;

        return new Item(
            Guid.NewGuid(),
            customId,
            customFieldValues,
            inventoryId,
            DateTime.UtcNow,
            creatorId,
            DateTime.UtcNow,
            creatorId
        );
    }

    public void Update(string customId, Guid updaterId, List<CustomFieldValue> customFieldValues)
    {
        UpdaterId = updaterId;
        UpdatedAt = DateTime.UtcNow;
        CustomId = customId;
        CustomFieldValues = customFieldValues;
    }
}

public static class ItemErrors
{
    public static readonly Error InventoryIdEmpty = new("ItemError.InventoryIdEmpty", "Inventory id is empty.");
    public static readonly Error CustomIdEmpty = new("ItemError.CustomIdEmpty", "Custom id is empty.");
    public static readonly Error CreatorIdEmpty = new("ItemError.CreatorIdEmpty", "Creator id is empty.");
}