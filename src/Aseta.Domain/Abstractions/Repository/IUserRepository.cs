using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Abstractions.Repository;

public interface IUserRepository : IRepository<UserApplication>
{
    Task<ICollection<UserApplication>> GetUsersPageAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
