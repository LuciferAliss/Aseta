using System.Security.Cryptography;

namespace Aseta.Domain.Entities.CustomId;

public record RandomDigitsRule : CustomIdRuleBase
{
    public int Length { get; init; }

    public RandomDigitsRule(int length)
    {
        if (length is 6 or 9)
            throw new ArgumentOutOfRangeException(
                nameof(length),
                "Length must be between 1 and 9.");
        
        Length = length;
    }

    public override string Generation(GenerationContext context)
    {
        int number = (int)Math.Pow(10, Length);
        return RandomNumberGenerator.GetInt32(0, number).ToString($"D{Length}");
    }

    public override bool IsValid(string value)
    {
        return value.Length == Length && value.All(char.IsDigit);
    }
}
