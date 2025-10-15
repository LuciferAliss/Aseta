using Aseta.Domain.Entities.CustomField;
using Aseta.Domain.Entities.CustomId;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Tags;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Inventories;

public class Inventory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string ImageUrl { get; private set; } = null!;
    public bool IsPublic { get; private set; }
    public ICollection<CustomFieldDefinition> CustomFields { get; private set; } = [];
    public virtual ICollection<Item> Items { get; private set; } = [];

    public int CategoryId { get; private set; }
    public virtual Category Category { get; private set; } = null!;

    public virtual ICollection<Tag> Tags { get; private set; } = [];
    public ICollection<CustomIdRuleBase> CustomIdRules { get; private set; } = [];
    public DateTime CreatedAt { get; private set; }

    public Guid CreatorId { get; private set; }
    public virtual UserApplication Creator { get; private set; } = null!;

    public virtual ICollection<InventoryUserRole> UserRoles { get; private set; } = [];

    private Inventory() { }

    private Inventory(Guid id, string name, string description, string imageUrl, bool isPublic, int categoryId, Guid creatorId)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        IsPublic = isPublic;
        CategoryId = categoryId;
        CreatedAt = DateTime.UtcNow;
        CreatorId = creatorId;
    }

    public static Inventory Create(string name, string description, string imageUrl, bool isPublic, int categoryId, Guid creatorId) =>
        new(Guid.NewGuid(), name, description, imageUrl, isPublic, categoryId, creatorId);

    public void Update(string name, string description, string imageUrl, bool isPublic) =>
        (Name, Description, ImageUrl, IsPublic) = (name, description, imageUrl, isPublic);

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
}