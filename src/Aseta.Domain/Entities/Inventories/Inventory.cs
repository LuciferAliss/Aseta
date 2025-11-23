using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Entities.Categories;
using Aseta.Domain.Entities.CustomField;
using Aseta.Domain.Entities.Inventories.CustomId;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Tags;
using Aseta.Domain.Entities.UserRoles;
using Aseta.Domain.Entities.Users;
using NpgsqlTypes;

namespace Aseta.Domain.Entities.Inventories;

public class Inventory : IEntity
{
    public Guid Id { get; private set; }
    public string InventoryName { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public bool IsPublic { get; private set; }
    public ICollection<CustomFieldDefinition> CustomFields { get; private set; } = [];
    public int ItemsCount { get; private set; }
    public virtual ICollection<Item> Items { get; private set; } = [];
    public Guid CategoryId { get; private set; }
    public virtual Category Category { get; private set; }
    public virtual ICollection<Tag> Tags { get; private set; } = [];
    public ICollection<CustomIdRuleBase> CustomIdRules { get; private set; } = [];
    public DateTime CreatedAt { get; private set; }
    public Guid CreatorId { get; private set; }
    public virtual ApplicationUser Creator { get; private set; }
    public virtual ICollection<InventoryRole> UserRoles { get; private set; } = [];
    public NpgsqlTsVector SearchVector { get; private set; }

    private Inventory()
    {
        InventoryName = null!;
        Description = null!;
        ImageUrl = null!;
        Creator = null!;
        Category = null!;
        SearchVector = null!;
    }

    private Inventory(Guid id, string name, string description, string imageUrl, bool isPublic, Guid categoryId, Guid creatorId, DateTime date)
    {
        Id = id;
        InventoryName = name;
        Description = description;
        ImageUrl = imageUrl;
        IsPublic = isPublic;
        CategoryId = categoryId;
        CreatedAt = date;
        CreatorId = creatorId;
        ItemsCount = 0;
        Creator = null!;
        Category = null!;
        SearchVector = null!;
    }

    public static Result<Inventory> Create(string name, string description, string imageUrl, bool isPublic, Guid categoryId, Guid creatorId, DateTime date) 
        => new Inventory(Guid.NewGuid(), name, description, imageUrl, isPublic, categoryId, creatorId, date);

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

    public void UpdateCustomFields(ICollection<CustomFieldDefinition> newFields)
    {
        var fieldCountsByType = newFields
                .GroupBy(field => field.Type)
                .ToDictionary(group => group.Key, group => group.Count());

        int maxFieldsPerType = 3;

        foreach (var entry in fieldCountsByType)
        {
            switch (entry.Key)
            {
                case CustomFieldType.SingleLineText:
                case CustomFieldType.Checkbox:
                case CustomFieldType.Date:
                case CustomFieldType.MultiLineText:
                case CustomFieldType.Number:
                    if (entry.Value > maxFieldsPerType)
                    {
                        throw new InvalidOperationException(
                            $"Cannot have more than {maxFieldsPerType} custom fields of type '{entry.Key}'. Found {entry.Value}."
                        );
                    }
                    break;
            }
        }

        CustomFields = newFields;
    }

    public Result Update(string name, string description, string imageUrl, bool isPublic)
    {
        InventoryName = name;
        Description = description;
        ImageUrl = imageUrl;
        IsPublic = isPublic;

        return Result.Success();
    }
}