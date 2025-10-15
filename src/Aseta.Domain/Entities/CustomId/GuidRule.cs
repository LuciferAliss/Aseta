using Aseta.Domain.Abstractions.Persistence;

namespace Aseta.Domain.Entities.CustomId;

public record GuidRule(string Format) : CustomIdRuleBase
{
    // GUID: "N", "D", "B", "P", "X"
    public override Task<string> Generation(GenerationContext context)
    {
        return Task.FromResult(Guid.NewGuid().ToString(Format));
    }

    public override bool IsValid(string value)
    {
        return Guid.TryParseExact(value, Format, out _);
    }
}

