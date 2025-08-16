using System;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Domain.User;

public class User : IdentityUser<Guid>
{
    private User(Guid id, string userName, string email)
    {
        Id = id;
        UserName = userName;
        Email = email;
    }

    private User() { }

    public static User Create(string UserName, string Email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(UserName);
        ArgumentException.ThrowIfNullOrWhiteSpace(Email);

        var user = new User(Guid.NewGuid(), UserName, Email);

        return user;
    }
}