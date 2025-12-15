using System;
using System.Data;
using Aseta.Domain.DTO.Items;
using FluentValidation;

namespace Aseta.Application.Items.GetPaginated;

internal sealed class GetItemsPaginatedQueryValidation : AbstractValidator<GetItemsPaginatedQuery>
{
    public GetItemsPaginatedQueryValidation()
    {
        RuleFor(x => x.InventoryId)
            .NotEqual(Guid.Empty).WithMessage("A valid InventoryId is required.");

        RuleFor(x => x.CreatorId)
            .NotEqual(Guid.Empty).WithMessage("A valid CreatorId is required.");

        RuleFor(x => x.UpdaterId)
            .NotEqual(Guid.Empty).WithMessage("A valid UpdaterId is required.");

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

        RuleFor(x => x.UpdatedAtFrom)
            .Must(d => d!.Value.Kind == DateTimeKind.Utc)
            .When(x => x.UpdatedAtFrom.HasValue)
            .WithMessage("UpdatedAtFrom must be in UTC.");

        RuleFor(x => x.UpdatedAtTo)
            .Must(d => d!.Value.Kind == DateTimeKind.Utc)
            .When(x => x.UpdatedAtTo.HasValue)
            .WithMessage("CreatedAtTo must be in UTC.");

        RuleFor(x => x.UpdatedAtTo)
            .GreaterThanOrEqualTo(x => x.UpdatedAtFrom.GetValueOrDefault())
            .When(x => x.UpdatedAtTo.HasValue && x.UpdatedAtFrom.HasValue)
            .WithMessage("UpdatedAtTo must be greater than or equal to UpdatedAtFrom.");

        RuleFor(x => x.SortBy)
            .NotEqual(SortBy.None).WithMessage("A valid SortBy is required.");
    }
}
