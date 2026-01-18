using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Comments.Add;

public sealed record AddCommentCommand(string Content, Guid InventoryId, Guid UserId) : ICommand;
