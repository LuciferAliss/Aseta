using Aseta.Domain.Abstractions.Repository;

namespace Aseta.Domain.Entities.CustomId;

public class DateRule : CustomIdRuleBase
{
    public string Format { get; set; }

    public override string Generation(IItemRepository itemRepository)
    {
        return DateTime.Now.ToString(Format);
    }

    public override bool IsValid(string value)
    {
        return DateTime.TryParseExact(value, Format, null, System.Globalization.DateTimeStyles.None, out _);
    }
}