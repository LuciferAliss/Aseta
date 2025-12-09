using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Categories;
using Aseta.Domain.Entities.Comments;
using Aseta.Domain.Entities.Inventories.CustomField;
using Aseta.Domain.Entities.Inventories.CustomId;
using Aseta.Domain.Entities.InventoryRoles;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Tags;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Inventories;

public class Inventory : Entity
{
    public const int MaxNameLength = 36;
    public const int MinNameLength = 3;
    public const int MaxDescriptionLength = 1000;

    public string Name { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public bool IsPublic { get; private set; }
    public ICollection<CustomFieldDefinition> CustomFields { get; private set; } = [];
    public int ItemsCount { get; private set; }
    public virtual ICollection<Item> Items { get; private set; } = [];
    public Guid CategoryId { get; private set; }
    public virtual Category Category { get; }
    public virtual ICollection<Tag> Tags { get; private set; } = [];
    public ICollection<CustomIdRuleBase> CustomIdRules { get; private set; } = [];
    public DateTime CreatedAt { get; private set; }
    public Guid CreatorId { get; private set; }
    public virtual User Creator { get; }
    public virtual ICollection<InventoryRole> UserRoles { get; private set; } = [];
    public virtual ICollection<Comment> Comments { get; private set; } = [];

    private Inventory() { }

    private Inventory(Guid id, string name, string description, Uri imageUrl, bool isPublic, Guid categoryId, Guid creatorId) : base(id)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl.ToString();
        IsPublic = isPublic;
        CategoryId = categoryId;
        CreatorId = creatorId;
        CreatedAt = DateTime.UtcNow;
        ItemsCount = 0;
        CustomIdRules = [new GuidRule("N")];
    }

    public static Result<Inventory> Create(string name, string description, Uri imageUrl, bool isPublic, Guid categoryId, Guid creatorId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return InventoryErrors.NameEmpty();
        }

        if (name.Length > MaxNameLength)
        {
            return InventoryErrors.NameTooLong(MaxNameLength);
        }

        if (name.Length < MinNameLength)
        {
            return InventoryErrors.NameTooShort(MinNameLength);
        }

        if (description.Length > MaxDescriptionLength)
        {
            return InventoryErrors.DescriptionTooLong(MaxDescriptionLength);
        }

        if (imageUrl is null)
        {
            return InventoryErrors.ImageUrlNull();
        }

        return new Inventory(Guid.NewGuid(), name, description, imageUrl, isPublic, categoryId, creatorId);
    }

    public void IncrementItemsCount() => ItemsCount++;

    public void DecrementItemsCount(int count = 1)
    {
        if (ItemsCount >= count)
        {
            ItemsCount -= count;
        }
        else
        {
            ItemsCount = 0;
        }
    }

    public Result UpdateCustomFields(ICollection<CustomFieldDefinition> newFields)
    {
        var fieldCountsByType = newFields
                .GroupBy(field => field.Type)
                .ToDictionary(group => group.Key, group => group.Count());

        int maxFieldsPerType = 3;

        foreach (KeyValuePair<CustomFieldType, int> entry in fieldCountsByType)
        {
            if (entry.Value > maxFieldsPerType)
            {
                return InventoryErrors.CustomFieldLimitExceeded(maxFieldsPerType, entry.Key.ToString(), entry.Value);
            }
        }

        CustomFields = newFields;
        return Result.Success();
    }

    public Result Update(string name, string description, Uri imageUrl, bool isPublic)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return InventoryErrors.NameEmpty();
        }

        if (name.Length > MaxNameLength || name.Length < MinNameLength)
        {
            return InventoryErrors.NameTooLong(MaxNameLength);
        }

        if (description.Length > MaxDescriptionLength)
        {
            return InventoryErrors.DescriptionTooLong(MaxDescriptionLength);
        }

        if (imageUrl is null)
        {
            return InventoryErrors.ImageUrlNull();
        }

        Name = name;
        Description = description;
        ImageUrl = imageUrl.ToString();
        IsPublic = isPublic;

        return Result.Success();
    }
}
