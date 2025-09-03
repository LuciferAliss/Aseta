using Aseta.Domain.Entities.CustomId;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Tags;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Inventories;

public class Inventory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }

    public bool IsPublic { get; private set; }

    public List<CustomFieldDefinition> CustomFields { get; private set; } = [];

    public virtual List<Item> Items { get; private set; } = [];

    public int CategoryId { get; private set; }
    public virtual Category Category { get; private set; }

    public virtual List<Tag> Tags { get; private set; } = [];
    
    public List<CustomIdRuleBase> CustomIdRules { get; set; } = [];

    public DateTime CreatedAt { get; private set; }

    public Guid CreatorId { get; private set; }
    public virtual UserApplication Creator { get; private set; }

    public virtual List<InventoryUserRole> UserRoles { get; private set; } = [];

    private Inventory() { }

    public Inventory(string name, string description, string imageUrl, bool isPublic, int categoryId, Guid creatorId)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        IsPublic = isPublic;
        CategoryId = categoryId;
        CreatedAt = DateTime.UtcNow;
        CreatorId = creatorId;
    }

    public static Inventory Create(string name, string description, string imageUrl, int categoryId, Guid creatorId, bool isPublic = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        return new Inventory(name, description, imageUrl, isPublic, categoryId, creatorId);
    }

    public void UpdateTags(List<Tag> tags)
    {
        Tags = tags;
    }

    public void UpdateCustomFields(List<CustomFieldDefinition> newFields)
    {
        CustomFields = newFields;
    }

    public void UpdateCategory(int categoryId)
    {
        CategoryId = categoryId;
    }
}