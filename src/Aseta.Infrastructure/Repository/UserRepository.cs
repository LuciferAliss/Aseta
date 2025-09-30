using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Repository;

public class UserRepository(AppDbContext context) 
: Repository<UserApplication>(context), IUserRepository
{
    public async Task<ICollection<UserApplication>> GetUsersPageAsync(
        int pageNumber,
         int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.OrderBy(u => u.UserName)
            .Skip((pageNumber - 1) * pageSize)  
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
