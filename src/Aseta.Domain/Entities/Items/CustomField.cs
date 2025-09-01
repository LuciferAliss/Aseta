using System;

namespace Aseta.Domain.Entities.Items;

public record CustomField(string Name, string? Value, string Type);