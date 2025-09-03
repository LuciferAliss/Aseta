using System.Globalization;
using System.Security.Cryptography;
using Aseta.Domain.Abstractions.Repository;

namespace Aseta.Domain.Entities.CustomId;

public class RandomNumberBitRule : CustomIdRuleBase
{
    public int CountBits { get; set; }
    public string Format { get; set; }

    public override Task<string> Generation(IItemRepository itemRepository, Guid inventoryId)
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