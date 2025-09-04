using System;
using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class UserRepository(AppDbContext context) : Repository<UserApplication, Guid>(context), IUserRepository
{
    public async Task<List<UserApplication>> GetUsersPageAsync(int pageNumber, int pageSize)
    {
        return await _dbSet.OrderBy(u => u.UserName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
