using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.CustomId;
using Aseta.Domain.Entities.Inventories;

namespace Aseta.Domain.Services.CustomId;

public class CustomIdService(IItemRepository itemRepository) : ICustomIdService
{
    private readonly IItemRepository _itemRepository = itemRepository;
    
    public async Task<Result<string>> GenerateAsync(
        ICollection<CustomIdRuleBase> customIdRule,
        Guid inventoryId,
        CancellationToken cancellationToken = default)
    {
        if (customIdRule.Count == 0) return Guid.NewGuid().ToString();

        if (inventoryId == Guid.Empty) return InventoryErrors.NotFound(inventoryId);

        

        var generationContext = new GenerationContext(
            DateTime.UtcNow,
            0);

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
