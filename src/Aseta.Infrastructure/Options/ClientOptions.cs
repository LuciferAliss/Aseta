using System.ComponentModel.DataAnnotations;

namespace Aseta.Infrastructure.Options;

public sealed class ClientOptions
{
    public const string SectionName = "Client";

    [Required(AllowEmptyStrings = false, ErrorMessage = "Url is required")]
    public string Url { get; set; }
}
