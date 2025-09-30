using Aseta.Domain.Abstractions;

namespace Aseta.Domain.Services.CustomId;

public static class CustomIdServiceErrors
{
    public static readonly Error CustomIdEmpty = Error.Problem("CustomIdService.CustomIdEmpty", "CustomId is empty.");
}