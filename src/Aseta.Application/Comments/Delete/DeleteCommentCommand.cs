using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Comments.Delete;

public sealed record DeleteCommentCommand(Guid CommentId, Guid UserId, bool IsAdmin) : ICommand;