using System.Globalization;
using System.Security.Cryptography;
using Aseta.Domain.Abstractions.Persistence;

namespace Aseta.Domain.Entities.CustomId;

public record RandomNumberBitRule(int CountBits, string Format) : CustomIdRuleBase
{
    public override Task<string> Generation(GenerationContext context)
    {
        if (CountBits == 32)
        {
            byte[] bytes = new byte[4];
            RandomNumberGenerator.Fill(bytes);

            return Task.FromResult(BitConverter.ToUInt32(bytes, 0).ToString(Format));
        }
        
        return Task.FromResult(RandomNumberGenerator.GetInt32(0, 1 << 20).ToString(Format)); 
    }

    public override bool IsValid(string value)
    {
        bool isParsed = uint.TryParse(value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out uint parsedValue);

        if (!isParsed) return false;

        if (CountBits == 32)
        {
            return true;
        }

        return parsedValue < (1 << CountBits);
    }
}