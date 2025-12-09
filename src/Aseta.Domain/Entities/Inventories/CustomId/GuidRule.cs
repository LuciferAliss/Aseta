using System.Linq;

namespace Aseta.Domain.Entities.Inventories.CustomId;

public record GuidRule : CustomIdRuleBase
{
    private static readonly string[] ValidFormats = ["N", "D", "B", "P", "X"];
    public string Format { get; init; }

    public GuidRule(string format)
    {
        if (!ValidFormats.Contains(format))
        {
            throw new ArgumentException($"Invalid GUID format. Must be one of: {string.Join(", ", ValidFormats)}");
        }

        Format = format;
    }

    public override string Generation(GenerationContext context)
    {
        return Guid.NewGuid().ToString(Format);
    }

    public override bool IsValid(string value)
    {
        return Guid.TryParseExact(value, Format, out _);
    }
}
