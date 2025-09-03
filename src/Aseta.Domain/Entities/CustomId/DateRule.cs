using Aseta.Domain.Abstractions.Repository;

namespace Aseta.Domain.Entities.CustomId;

public class DateRule : CustomIdRuleBase
{
    public string Format { get; set; }

    public override Task<string> Generation(IItemRepository itemRepository, Guid inventoryId)
    {
        return Task.FromResult(DateTime.Now.ToString(Format));
    }

    public override bool IsValid(string value)
    {
        return DateTime.TryParseExact(value, Format, null, System.Globalization.DateTimeStyles.None, out _);
    }
}