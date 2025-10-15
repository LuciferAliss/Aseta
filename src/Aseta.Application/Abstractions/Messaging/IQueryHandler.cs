using Aseta.Domain.Abstractions.Primitives;

namespace Aseta.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}