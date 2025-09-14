using System.Text.Json.Serialization;

namespace Aseta.Application.DTO.CustomId;

[JsonDerivedType(typeof(DateRuleResponse), typeDiscriminator: "date")]
[JsonDerivedType(typeof(GuidRuleResponse), typeDiscriminator: "guid")]
[JsonDerivedType(typeof(FixedTextRuleResponse), typeDiscriminator: "fixed_text")]
[JsonDerivedType(typeof(SequenceRuleResponse), typeDiscriminator: "sequence")]
[JsonDerivedType(typeof(RandomDigitsRuleResponse), typeDiscriminator: "random_digits")]
[JsonDerivedType(typeof(RandomNumberBitRuleResponse), typeDiscriminator: "random_bits")]
public abstract record CustomIdRulePartResponse
{
    [JsonPropertyName("$type")]
    public string Type { get; init; }
    public int Order { get; init; }
}

public record DateRuleResponse : CustomIdRulePartResponse
{
    public string Format { get; init; }
}

public record GuidRuleResponse : CustomIdRulePartResponse
{
    public string Format { get; init; }
}

public record FixedTextRuleResponse : CustomIdRulePartResponse
{
    public string Text { get; init; }
}

public record SequenceRuleResponse : CustomIdRulePartResponse
{
    public int Padding { get; init; }
}

public record RandomDigitsRuleResponse : CustomIdRulePartResponse
{
    public int Length { get; init; }
}

public record RandomNumberBitRuleResponse : CustomIdRulePartResponse
{
    public int CountBits { get; init; }
    public string Format { get; init; }
}