using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.Application.Abstractions.Authentication;

public interface IUserSessionChecker
{
    Task<Result> CheckAsync(Guid id, CancellationToken cancellationToken = default);
}
