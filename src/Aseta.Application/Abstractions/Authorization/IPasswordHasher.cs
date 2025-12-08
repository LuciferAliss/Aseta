using System;

namespace Aseta.Application.Abstractions.Authorization;

public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string password, string passwordHash);
}