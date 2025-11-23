namespace Aseta.Application.Inventories.Get.Contracts.CustomIdRule;

public sealed record RandomNumberBitRuleResponse(
    int CountBits,
    string Format) : CustomIdRuleResponse;