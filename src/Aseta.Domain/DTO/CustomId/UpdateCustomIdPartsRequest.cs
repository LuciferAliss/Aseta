using System.Text.Json.Serialization;

namespace Aseta.Domain.DTO.CustomId;

public record UpdateCustomIdPartsRequest
{
    public List<CustomIdRulePartRequest> CustomIdRuleParts { get; init; }
}

[JsonDerivedType(typeof(FixedTextRulePartRequest), typeDiscriminator: "fixed_text")]
[JsonDerivedType(typeof(SequenceRulePartRequest), typeDiscriminator: "sequence")]
[JsonDerivedType(typeof(DateRulePartRequest), typeDiscriminator: "date")]
[JsonDerivedType(typeof(GuidRulePartRequest), typeDiscriminator: "guid")]
[JsonDerivedType(typeof(RandomDigitsRulePartRequest), typeDiscriminator: "random_digits")]
[JsonDerivedType(typeof(RandomNumberBitRulePartRequest), typeDiscriminator: "random_bits")]
public abstract record CustomIdRulePartRequest
{
    public int Order { get; init; }
}

public record FixedTextRulePartRequest : CustomIdRulePartRequest
{
    public string Text { get; init; }
}

public record SequenceRulePartRequest : CustomIdRulePartRequest
{
    public int Padding { get; init; }
}

public record DateRulePartRequest : CustomIdRulePartRequest
{
    public string Format { get; init; }
}

public record GuidRulePartRequest : CustomIdRulePartRequest
{
    public string Format { get; init; }
}

public record RandomDigitsRulePartRequest : CustomIdRulePartRequest
{
    public int Length { get; init; }
}

public record RandomNumberBitRulePartRequest : CustomIdRulePartRequest
{
    public int CountBits { get; init; }
    public string Format { get; init; }
}