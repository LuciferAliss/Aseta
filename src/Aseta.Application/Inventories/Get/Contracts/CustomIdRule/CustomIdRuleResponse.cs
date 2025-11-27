using System.Text.Json.Serialization;

namespace Aseta.Application.Inventories.Get.Contracts.CustomIdRule;

[JsonDerivedType(typeof(GuidRuleResponse), typeDiscriminator: "guid")]
[JsonDerivedType(typeof(FixedTextRuleResponse), typeDiscriminator: "fixed_text")]
[JsonDerivedType(typeof(SequenceRuleResponse), typeDiscriminator: "sequence")]
public abstract record CustomIdRuleResponse
{
    [JsonPropertyName("$type")]
    public required string Type { get; init; }
    public int Order { get; init; }
}
