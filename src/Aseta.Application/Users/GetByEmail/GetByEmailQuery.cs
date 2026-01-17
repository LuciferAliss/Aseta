using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Users.GetByEmail;

public sealed record GetByEmailQuery(string Email) : IQuery<UserResponse>;
