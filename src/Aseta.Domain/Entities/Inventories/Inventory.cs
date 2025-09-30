using Aseta.Domain.Entities.CustomField;
using Aseta.Domain.Entities.CustomId;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Tags;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Inventories;

public class Inventory
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public bool IsPublic { get; set; }
    public ICollection<CustomFieldDefinition> CustomFields { get; set; } = [];
    public virtual ICollection<Item> Items { get; set; } = [];

    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = [];
    public ICollection<CustomIdRuleBase> CustomIdRules { get; set; } = [];
    public DateTime CreatedAt { get; set; }

    public Guid CreatorId { get; set; }
    public virtual UserApplication Creator { get; set; } = null!;

    public virtual ICollection<InventoryUserRole> UserRoles { get; set; } = [];

    public Inventory(string name, string description, string imageUrl, bool isPublic, int categoryId, Guid creatorId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        IsPublic = isPublic;
        CategoryId = categoryId;
        CreatedAt = DateTime.UtcNow;
        CreatorId = creatorId;
    }
    
    public Inventory(Guid id, string name, string description, string imageUrl, bool isPublic, int categoryId, Guid creatorId)
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