using System;
using Aseta.Domain.Entities.Comments;
using FluentValidation;

namespace Aseta.Application.Comments.Add;

internal sealed class AddCommentCommandValidation : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidation()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(x => x.Content)
            .MaximumLength(Comment.MaxContentLength).WithMessage($"The content cannot exceed {Comment.MaxContentLength} characters.");

        RuleFor(x => x.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("InventoryId is required.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("UserId is required.");
    }
}
