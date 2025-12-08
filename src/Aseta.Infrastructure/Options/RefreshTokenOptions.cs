using System.ComponentModel.DataAnnotations;

namespace Aseta.Infrastructure.Options;

public class RefreshTokenOptions
{
    public const string SectionName = "RefreshToken";

    [Required(ErrorMessage = "RefreshToken:ExpirationInDays is required.")]
    [Range(1, 365, ErrorMessage = "RefreshToken:ExpirationInDays must be between 1 and 365 days.")]
    public int ExpirationInDays { get; init; }
}
