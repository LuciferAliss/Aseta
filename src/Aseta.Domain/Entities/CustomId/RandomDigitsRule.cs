using System.Security.Cryptography;
using Aseta.Domain.Abstractions.Repository;

namespace Aseta.Domain.Entities.CustomId;

public class RandomDigitsRule : CustomIdRuleBase
{
    public int Length { get; set; }

    public override Task<string> Generation(IItemRepository itemRepository, Guid inventoryId)
    {
        int number = (int)Math.Pow(10, Length);
        return Task.FromResult(RandomNumberGenerator.GetInt32(0, number).ToString($"D{Length}"));
    }

    public override bool IsValid(string value)
    {
        return int.TryParse(value, out _) && value.Length == Length;
    }
}