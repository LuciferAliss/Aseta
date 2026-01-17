using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Users.SearchUser;

internal sealed class SearchUserQueryHandler(IUserRepository userRepository) : IQueryHandler<SearchUserQuery, UsersResponse>
{
    public async Task<Result<UsersResponse>> Handle(SearchUserQuery query, CancellationToken cancellationToken)
    {
        User? user = await userRepository.SearchByEmailAsync(query.Email);

        throw new NotImplementedException();
    }
}
