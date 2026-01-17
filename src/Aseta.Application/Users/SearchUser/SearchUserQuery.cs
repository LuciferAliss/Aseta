using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Users.SearchUser;

public sealed record SearchUserQuery(string SearchTerm) : IQuery<UsersResponse>;
