using Aseta.Domain.Abstractions.Persistence;

namespace Aseta.Domain.Entities.CustomId;

public record SequenceRule(int Padding) : CustomIdRuleBase
{
    public override async Task<string> Generation(GenerationContext context)
    {
        int count = await context.ItemRepository.LastItemPosition(context.InventoryId) + 1;
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