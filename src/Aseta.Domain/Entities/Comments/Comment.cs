using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Entities.Comments;

public class Comment : Entity
{
    public const int MaxContentLength = 1000;

    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid UserId { get; private set; }
    public virtual User User { get; }
    public Guid InventoryId { get; private set; }
    public virtual Inventory Inventory { get; }
    public virtual ICollection<Like> Likes { get; private set; } = [];

    private Comment() { }

    private Comment(Guid id, string content, Guid userId, Guid inventoryId) : base(id)
    {
        Content = content;
        UserId = userId;
        InventoryId = inventoryId;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<Comment> Create(string content, Guid userId, Guid inventoryId)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return CommentErrors.ContentEmpty();
        }

        if (content.Length > MaxContentLength)
        {
            return CommentErrors.ContentTooLong(MaxContentLength);
        }

        return new Comment(Guid.NewGuid(), content, userId, inventoryId);
    }

    public Result UpdateContent(string newContent)
    {
        if (string.IsNullOrWhiteSpace(newContent))
        {
            return CommentErrors.ContentEmpty();
        }
        if (newContent.Length > MaxContentLength)
        {
            return CommentErrors.ContentTooLong(MaxContentLength);
        }

        Content = newContent;
        return Result.Success();
    }

    public void AddLike(Like like)
    {
        if (Likes.Any(l => l.UserId == like.UserId))
        {
            return;
        }
        Likes.Add(like);
    }

    public void RemoveLike(Like like)
    {
        if (like is not null)
        {
            Likes.Remove(like);
        }
    }
}
