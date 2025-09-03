using System.Text.Json.Serialization;
using Aseta.Domain.Abstractions.Repository;

namespace Aseta.Domain.Entities.CustomId;

[JsonDerivedType(typeof(FixedTextRule), typeDiscriminator: "fixed_text")]
[JsonDerivedType(typeof(SequenceRule), typeDiscriminator: "sequence")]
[JsonDerivedType(typeof(DateRule), typeDiscriminator: "date")]
[JsonDerivedType(typeof(GuidRule), typeDiscriminator: "guid")]
[JsonDerivedType(typeof(RandomDigitsRule), typeDiscriminator: "random_digits")]
[JsonDerivedType(typeof(RandomNumberBitRule), typeDiscriminator: "random_bits")]
public abstract class CustomIdRuleBase
{
    public int Order { get; set; }
    abstract public string Generation(IItemRepository itemRepository);
    abstract public bool IsValid(string value);
}