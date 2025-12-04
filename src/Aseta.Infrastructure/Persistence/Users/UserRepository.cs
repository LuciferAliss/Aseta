using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Persistence.Common;

namespace Aseta.Infrastructure.Persistence.Users;

public sealed class UserRepository(AppDbContext context) : Repository<ApplicationUser>(context), IUserRepository;