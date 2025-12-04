using Aseta.Domain.Abstractions.Primitives.Errors;

namespace Aseta.Infrastructure.DomainEvents;

internal static class DomainEventsDispatcherErrors
{
    internal static Error HandlersFailed => Error.Failure(
        "DomainEventsDispatcher.HandlersFailed",
        "One or more domain event handlers failed to process the domain event.");
}