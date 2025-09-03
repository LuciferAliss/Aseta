using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.Entities.CustomId;

namespace Aseta.Domain.Services;

public class CustomIdService(IItemRepository itemRepository) : ICustomIdService
{
    private readonly IItemRepository _itemRepository = itemRepository;

    public string GenerateAsync(List<CustomIdRuleBase> customIdRule, Guid inventoryId)
    {
        if (customIdRule.Count == 0) return Guid.NewGuid().ToString();

        var customId = customIdRule.Select(async r => await r.Generation(_itemRepository, inventoryId));
        return string.Join("-", customId);
    }

    public bool IsValid(string customId, List<CustomIdRuleBase> customIdRule)
    {
        return customIdRule.Any(r => r.IsValid(customId));
    }
}