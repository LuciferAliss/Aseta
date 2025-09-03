using Aseta.Domain.Abstractions.Repository;

namespace Aseta.Domain.Entities.CustomId;

public class FixedTextRule : CustomIdRuleBase
{
    public string Text { get; set; }

    public override Task<string> Generation(IItemRepository itemRepository, Guid inventoryId)
    {
        return Task.FromResult(Text);
    }

    public override bool IsValid(string value)
    {
        return true;
    }
}