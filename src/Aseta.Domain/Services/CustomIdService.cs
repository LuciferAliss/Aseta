using Aseta.Domain.Abstractions;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.CustomId;
using Aseta.Domain.Errors;

namespace Aseta.Domain.Services;

public class CustomIdService(IItemRepository itemRepository) : ICustomIdService
{
    private readonly IItemRepository _itemRepository = itemRepository;

    public async Task<Result<string>> GenerateAsync(List<CustomIdRuleBase> customIdRule, Guid inventoryId)
    {
        if (customIdRule.Count == 0) return Guid.NewGuid().ToString();

        if (inventoryId == Guid.Empty) return Result.Failure<string>(CustomIdServiceErrors.InventoryIdEmpty);

        var customIdParts = await Task.WhenAll(customIdRule.Select(r => r.Generation(_itemRepository, inventoryId)));
        var customId = string.Join("-", customIdParts);

        if (string.IsNullOrWhiteSpace(customId)) return Result.Failure<string>(CustomIdServiceErrors.CustomIdEmpty);

        return Result.Success(customId); 
    }

    public Result<bool> IsValid(string customId, List<CustomIdRuleBase> customIdRule)
    {
        if (string.IsNullOrWhiteSpace(customId)) return CustomIdServiceErrors.CustomIdEmpty;

        if (customIdRule.Count == 0) return true;

        return customIdRule.Any(r => r.IsValid(customId));
    }
}
