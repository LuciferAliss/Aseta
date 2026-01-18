using System.Linq.Expressions;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Abstractions.Persistence;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(
        string email,
        bool trackChanges = default,
        CancellationToken cancellationToken = default,
        params Expression<Func<User, object>>[] includeProperties);

    Task<ICollection<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

}