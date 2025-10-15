namespace Aseta.Application.Contracts.CustomIdRule;

public sealed record FixedTextRuleResponse(
    string Text) : CustomIdRuleResponse;
