using System;
using Aseta.Domain.DTO.Comments;
using FluentValidation;

namespace Aseta.Application.Comments.GetPaginated;

internal sealed class GetCommentsPaginatedQueryValidation : AbstractValidator<GetCommentsPaginatedQuery>
{
    public GetCommentsPaginatedQueryValidation()
    {
        RuleFor(x => x.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("A valid InventoryId is required.");

        RuleFor(x => x.SortBy)
            .NotEqual(SortBy.None).WithMessage("A valid SortBy is required.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PageSize must be greater than 0.");
    }
}
