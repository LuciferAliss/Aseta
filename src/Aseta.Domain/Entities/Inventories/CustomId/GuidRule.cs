namespace Aseta.Domain.Entities.Inventories.CustomId;

public record GuidRule(string Format) : CustomIdRuleBase
{
    // GUID: "N", "D", "B", "P", "X"
    public override string Generation(GenerationContext context)
    {
        return Guid.NewGuid().ToString(Format);
    }

    public override bool IsValid(string value)
    {
        return Guid.TryParseExact(value, Format, out _);
    }
}
