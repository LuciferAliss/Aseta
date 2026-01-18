using System;
using FluentValidation;

namespace Aseta.Application.Comments.Delete;

internal sealed class DeleteCommentCommandValidation : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidation()
    {
        RuleFor(x => x.CommentId)
            .NotEqual(Guid.Empty).WithMessage("A valid CommentId is required.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("A valid UserId is required.");
    }
}
