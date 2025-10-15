namespace Aseta.Application.Contracts.CustomIdRule;

public sealed record RandomDigitsRuleResponse(
    int Length) : CustomIdRuleResponse;
