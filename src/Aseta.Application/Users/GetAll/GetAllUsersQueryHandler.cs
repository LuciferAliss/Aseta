using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Users.GetAll;

internal sealed class GetAllUsersQueryHandler(IUserRepository userRepository) : IQueryHandler<GetAllUsersQuery, UsersResponse>
{
    public async Task<Result<UsersResponse>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<User> users = await userRepository.GetAllAsync(cancellationToken: cancellationToken);

        var response = new UsersResponse(users.Select(u => new UserResponse(u.Id, u.UserName, u.Email, u.IsLocked, u.Role.ToString())).ToList());

        return response;
    }
}
