using System;

namespace Aseta.Application.Users.Contracts;

public sealed record AccessTokenGenerationRequest(Guid Id,  string Email);
