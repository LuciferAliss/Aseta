namespace Aseta.Application.Contracts.CustomIdRule;

public sealed record RandomNumberBitRuleResponse(
    int CountBits,
    string Format) : CustomIdRuleResponse;