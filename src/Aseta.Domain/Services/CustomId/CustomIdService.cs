using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.Inventories.CustomId;

namespace Aseta.Domain.Services.CustomId;

public class CustomIdService(IItemRepository itemRepository) : ICustomIdService
{
    public async Task<Result<string>> GenerateAsync(
        Guid inventoryId,
        Guid itemId,
        ICollection<CustomIdRuleBase> customIdRule,
        CancellationToken cancellationToken = default)
    {
        if (customIdRule.Count == 0) return Guid.NewGuid().ToString();

        var itemSequence = await itemRepository.GetItemSequenceNumberAsync(itemId, inventoryId, cancellationToken);

        var generationContext = new GenerationContext()
        {
            ItemSequence = itemSequence
        };

        var customIdParts = customIdRule.Select(r => r.Generation(generationContext));
        var customId = string.Join("-", customIdParts);

        if (string.IsNullOrWhiteSpace(customId)) 
            return CustomIdServiceErrors.CustomIdEmpty();

        return customId;
    }

    public Result<bool> IsValid(
        string customId,
        ICollection<CustomIdRuleBase> customIdRule)
    {
        if (string.IsNullOrWhiteSpace(customId))
            return CustomIdServiceErrors.CustomIdEmpty();

        if (customIdRule.Count == 0)
            return Result.Success(true);

        var parts = customId.Split('-');
        if (parts.Length != customIdRule.Count)
        {
            return CustomIdServiceErrors.TemplateMismatch();
        }

        var allPartsValid = customIdRule
            .Zip(parts, (rule, part) => rule.IsValid(part))
            .All(isValid => isValid);

        return Result.Success(allPartsValid);
    }
}
