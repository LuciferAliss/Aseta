using System.Text.Json.Serialization;

namespace Aseta.Application.Contracts.CustomIdRule;

[JsonDerivedType(typeof(DateRuleResponse), typeDiscriminator: "date")]
[JsonDerivedType(typeof(GuidRuleResponse), typeDiscriminator: "guid")]
[JsonDerivedType(typeof(FixedTextRuleResponse), typeDiscriminator: "fixed_text")]
[JsonDerivedType(typeof(SequenceRuleResponse), typeDiscriminator: "sequence")]
[JsonDerivedType(typeof(RandomDigitsRuleResponse), typeDiscriminator: "random_digits")]
[JsonDerivedType(typeof(RandomNumberBitRuleResponse), typeDiscriminator: "random_bits")]
public abstract record CustomIdRuleResponse
{
    [JsonPropertyName("$type")]
    public required string Type { get; init; }
    public int Order { get; init; }
}
