using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Users.GetAll;

[Authorize(UserRole.Admin)]
public sealed record GetAllUsersQuery : IQuery<UsersResponse>;
