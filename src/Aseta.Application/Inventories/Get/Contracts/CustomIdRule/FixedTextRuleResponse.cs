namespace Aseta.Application.Inventories.Get.Contracts.CustomIdRule;

public sealed record FixedTextRuleResponse(
    string Text) : CustomIdRuleResponse;
