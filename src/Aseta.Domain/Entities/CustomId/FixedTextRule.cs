using Aseta.Domain.Abstractions.Persistence;

namespace Aseta.Domain.Entities.CustomId;

public record FixedTextRule(string Text) : CustomIdRuleBase
{
    public override Task<string> Generation(GenerationContext context)
    {
        return Task.FromResult(Text);
    }

    public override bool IsValid(string value)
    {
        return value == Text;
    }
}