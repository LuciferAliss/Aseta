using System;

namespace Aseta.Infrastructure.Options;

public sealed record JwtOptions
{
   public const string JWT_OPTIONS_KEY = "JwtOptions";

    public required string Secret { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public int ExpirationInMinutes { get; set; } 
}
