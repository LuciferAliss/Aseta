using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure.Database;

namespace Aseta.Infrastructure.Repository;

internal sealed class UserRepository(AppDbContext context) : Repository<UserApplication>(context), IUserRepository;