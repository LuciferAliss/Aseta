using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Comments;

public class Comment
{
    public Guid Id { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Guid UserId { get; private set; }
    public virtual ApplicationUser User { get; private set; }

    public Guid InventoryId { get; private set; }
    public virtual Inventory Inventory { get; private set; }

    public int Lick { get; private set; }

    private Comment() { }

    private Comment(Guid id, string content, DateTime createdAt, Guid userId, Guid inventoryId, int lick)
    {
        Id = id;
        Content = content;
        CreatedAt = createdAt;
        UserId = userId;
        InventoryId = inventoryId;
        Lick = lick;
    }

    public static Comment Create(Guid id, string content, DateTime createdAt, Guid userId, Guid inventoryId, int lick)
    {   
        return new Comment(id, content, createdAt, userId, inventoryId, lick);
    }
}
