namespace Aseta.Application.Inventories.Get.Contracts.CustomIdRule;

public sealed record RandomDigitsRuleResponse(
    int Length) : CustomIdRuleResponse;
