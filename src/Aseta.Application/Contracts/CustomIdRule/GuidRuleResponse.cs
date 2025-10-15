namespace Aseta.Application.Contracts.CustomIdRule;

public sealed record GuidRuleResponse(
    string Format) : CustomIdRuleResponse;
