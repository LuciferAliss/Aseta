using Aseta.Domain.Abstractions.Persistence;

namespace Aseta.Domain.Entities.CustomId;

public record DateRule(string Format) : CustomIdRuleBase
{
    public override Task<string> Generation(GenerationContext context)
    {
        return Task.FromResult(DateTime.Now.ToString(Format));
    }

    public override bool IsValid(string value)
    {
        return DateTime.TryParseExact(value, Format, null, System.Globalization.DateTimeStyles.None, out _);
    }
}