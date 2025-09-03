using Aseta.Domain.Abstractions.Repository;

namespace Aseta.Domain.Entities.CustomId;

public class SequenceRule : CustomIdRuleBase
{
    public int Padding { get; set; }

    public override async Task<string> Generation(IItemRepository itemRepository, Guid inventoryId)
    {
        int count = await itemRepository.LastItemPosition(inventoryId) + 1;
        return count.ToString($"D{Padding}");
    }

    public override bool IsValid(string value)
    {
        if (!int.TryParse(value, out int parsedNumber) || parsedNumber < 0)
        {
            return false;
        }
        
        return value == parsedNumber.ToString($"D{Padding}");
    }
}