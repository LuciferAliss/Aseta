using System.Text.Json.Serialization;

namespace Aseta.Domain.Entities.CustomId;

[JsonDerivedType(typeof(FixedTextRule), typeDiscriminator: "fixed_text")]
[JsonDerivedType(typeof(SequenceRule), typeDiscriminator: "sequence")]
[JsonDerivedType(typeof(DateRule), typeDiscriminator: "date")]
[JsonDerivedType(typeof(GuidRule), typeDiscriminator: "guid")]
[JsonDerivedType(typeof(RandomDigitsRule), typeDiscriminator: "random_digits")]
[JsonDerivedType(typeof(RandomNumberBitRule), typeDiscriminator: "random_bits")]
public abstract record CustomIdRuleBase
{
    public int Order { get; init; }
    abstract public Task<string> Generation(GenerationContext context);
    abstract public bool IsValid(string value);
}