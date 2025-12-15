using System;
using System.Data;
using Aseta.Domain.DTO.Inventories;
using FluentValidation;

namespace Aseta.Application.Inventories.GetPaginated;

internal sealed class GetInventoriesPaginatedQueryValidation : AbstractValidator<GetInventoriesPaginatedQuery>
{
    public GetInventoriesPaginatedQueryValidation()
    {
        RuleFor(x => x.CreatedAtFrom)
            .Must(d => d!.Value.Kind == DateTimeKind.Utc)
            .When(x => x.CreatedAtFrom.HasValue)
            .WithMessage("CreatedAtFrom must be in UTC.");

        RuleFor(x => x.CreatedAtTo)
            .Must(d => d!.Value.Kind == DateTimeKind.Utc)
            .When(x => x.CreatedAtTo.HasValue)
            .WithMessage("CreatedAtTo must be in UTC.");

        RuleFor(x => x.CreatedAtTo)
            .GreaterThanOrEqualTo(x => x.CreatedAtFrom.GetValueOrDefault())
            .When(x => x.CreatedAtTo.HasValue && x.CreatedAtFrom.HasValue)
            .WithMessage("CreatedAtTo must be greater than or equal to CreatedAtFrom.");

        RuleForEach(x => x.TagIds)
            .NotEqual(Guid.Empty).WithMessage("A valid TagId is required.");

        RuleForEach(x => x.CategoryIds)
            .NotEqual(Guid.Empty).WithMessage("A valid CategoryId is required.");

        RuleFor(x => x.MinItemsCount)
            .GreaterThanOrEqualTo(0).WithMessage("MinItemsCount must be greater than or equal to 0.");

        RuleFor(x => x.MaxItemsCount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxItemsCount.HasValue)
            .WithMessage("MaxItemsCount must be greater than or equal to 0.");

        RuleFor(x => x.MaxItemsCount)
            .GreaterThanOrEqualTo(x => x.MinItemsCount.GetValueOrDefault())
            .When(x => x.MaxItemsCount.HasValue && x.MinItemsCount.HasValue)
            .WithMessage("MaxItemsCount must be greater than or equal to MinItemsCount.");

        RuleFor(x => x.SortBy)
            .NotEqual(SortBy.None).WithMessage("A valid SortBy is required.");
    }
}
