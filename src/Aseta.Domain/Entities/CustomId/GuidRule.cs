using Aseta.Domain.Abstractions.Repository;

namespace Aseta.Domain.Entities.CustomId;

public class GuidRule : CustomIdRuleBase
{
    // GUID: "N", "D", "B", "P", "X"
    public string Format { get; set; }

    public override Task<string> Generation(IItemRepository itemRepository, Guid inventoryId)
    {
        return Task.FromResult(Guid.NewGuid().ToString(Format));
    }

    public override bool IsValid(string value)
    {
        return Guid.TryParseExact(value, Format, out _);
    }
}

