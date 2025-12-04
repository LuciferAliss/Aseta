using System;
using System.Globalization;

namespace Aseta.Domain.Entities.Inventories.CustomId;

public record SequenceRule(int Padding) : CustomIdRuleBase
{
    public override string Generation(GenerationContext context)
    {
        if (Padding < 0)
        {
            throw new ArgumentException("Padding cannot be negative.");
        }

        return context.ItemSequence.ToString($"D{Padding}", CultureInfo.InvariantCulture);
    }

    public override bool IsValid(string value)
    {
        if (!int.TryParse(value, out int parsedNumber) || parsedNumber < 0)
        {
            return false;
        }

        return value == parsedNumber.ToString($"D{Padding}", CultureInfo.InvariantCulture);
    }
}