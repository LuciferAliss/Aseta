using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Users.GetByEmail;

internal sealed class GetByEmailQueryHandler(IUserRepository userRepository) : IQueryHandler<GetByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetByEmailQuery query, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByEmailAsync(query.Email);

        if (user is null)
        {
            return UserErrors.NotFound(query.Email);
        }

        var response = new UserResponse(user.Id, user.UserName, user.Email);

        return response;
    }
}
