using System.Security.Cryptography;
using Aseta.Domain.Abstractions.Persistence;

namespace Aseta.Domain.Entities.CustomId;

public record RandomDigitsRule(int Length) : CustomIdRuleBase
{
    public override Task<string> Generation(GenerationContext context)
    {
        int number = (int)Math.Pow(10, Length);
        return Task.FromResult(RandomNumberGenerator.GetInt32(0, number).ToString($"D{Length}"));
    }

    public override bool IsValid(string value)
    {
        return int.TryParse(value, out _) && value.Length == Length;
    }
}