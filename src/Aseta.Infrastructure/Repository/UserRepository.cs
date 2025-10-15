using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure.Database;

namespace Aseta.Infrastructure.Repository;

public class UserRepository(AppDbContext context) : Repository<UserApplication>(context), IUserRepository;