using System.Linq;

namespace Aseta.Domain.Entities.Inventories.CustomId;

public record GuidRule(string Format) : CustomIdRuleBase
{
    private static readonly string[] ValidFormats = ["N", "D", "B", "P", "X"];

    public override string Generation(GenerationContext context)
    {
        if (!ValidFormats.Contains(Format))
        {
            throw new ArgumentException($"Invalid GUID format. Must be one of: {string.Join(", ", ValidFormats)}");
        }

        return Guid.NewGuid().ToString(Format);
    }

    public override bool IsValid(string value)
    {
        return Guid.TryParseExact(value, Format, out _);
    }
}
