using Aseta.Domain.Abstractions.Repository;

namespace Aseta.Domain.Entities.CustomId;

public class FixedTextRule : CustomIdRuleBase
{
    public string Text { get; set; }

    public override string Generation(IItemRepository itemRepository)
    {
        return Text;
    }

    public override bool IsValid(string value)
    {
        return true;
    }
}