using Aseta.Domain.Abstractions.Repository;

namespace Aseta.Domain.Entities.CustomId;

public class GuidRule : CustomIdRuleBase
{
    // GUID: "N", "D", "B", "P", "X"
    public string Format { get; set; }

    public override string Generation(IItemRepository itemRepository)
    {
        return Guid.NewGuid().ToString(Format);
    }

    public override bool IsValid(string value)
    {
        return Guid.TryParseExact(value, Format, out _);
    }
}

