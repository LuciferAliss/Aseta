namespace Aseta.Domain.Entities.CustomId;

public record FixedTextRule(string Text) : CustomIdRuleBase
{
    public override string Generation(GenerationContext context)
    {
        return Text;
    }

    public override bool IsValid(string value)
    {
        return value == Text;
    }
}