namespace Aseta.Domain.Entities.CustomId;

public record DateRule(string Format) : CustomIdRuleBase
{
    public override string Generation(GenerationContext context)
    {
        return context.GenerationTime.ToString(Format);
    }

    public override bool IsValid(string value)
    {
        return DateTime.TryParseExact(value, Format, null, System.Globalization.DateTimeStyles.None, out _);
    }
}