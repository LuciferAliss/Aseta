using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Abstractions.Primitives.Events;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.InventoryRoles;

namespace Aseta.Domain.Entities.Users;

public class User : Entity
{
    public const int MaxUserNameLength = 32;
    public const int MinUserNameLength = 3;
    public const int MaxEmailLength = 256;
    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 128;

    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsLocked { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public virtual ICollection<Inventory> Inventories { get; }
    public virtual ICollection<InventoryRole> InventoryUserRoles { get; }
    public virtual ICollection<UserSession> UserSessions { get; }

    private User() { }

    private User(Guid id, string userName, string email, string passwordHash, UserRole role) : base(id)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        Role = role;
        IsLocked = false;
    }

    public static Result<User> Create(string userName, string email, string passwordHash, UserRole role = UserRole.User)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return UserErrors.UserNameEmpty();
        }

        if (userName.Length > MaxUserNameLength)
        {
            return UserErrors.UserNameTooLong(MaxUserNameLength);
        }

        if (userName.Length < MinUserNameLength)
        {
            return UserErrors.UserNameTooShort(MinUserNameLength);
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return UserErrors.EmailEmpty();
        }

        if (email.Length > MaxEmailLength)
        {
            return UserErrors.EmailTooLong(MaxEmailLength);
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return UserErrors.PasswordEmpty();
        }

        return new User(Guid.NewGuid(), userName, email, passwordHash, role);
    }

    public Result Update(string userName, string email, string passwordHash) // ! TODO: add validation
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}