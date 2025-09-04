using System;
using Aseta.Domain.Entities.Users;

namespace Aseta.Domain.Abstractions.Repository;

public interface IUserRepository : IRepository<UserApplication, Guid>
{
    Task<List<UserApplication>> GetUsersPageAsync(int pageNumber, int pageSize);
}
