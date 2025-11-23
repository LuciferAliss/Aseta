namespace Aseta.Application.Inventories.Get.Contracts.CustomIdRule;

public sealed record GuidRuleResponse(
    string Format) : CustomIdRuleResponse;
