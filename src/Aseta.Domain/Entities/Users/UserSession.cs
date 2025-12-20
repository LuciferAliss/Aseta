using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Abstractions.Primitives.Results;
using System;

namespace Aseta.Domain.Entities.Users;

public class UserSession : Entity
{
    public const int MaxTokenLength = 256;
    public const int MaxDeviceIdLength = 64;
    public const int MaxDeviceNameLength = 128;

    public string Token { get; private set; }
    public Guid UserId { get; private set; }
    public virtual User User { get; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public string DeviceId { get; private set; }
    public string DeviceName { get; private set; }

    private UserSession() { }

    private UserSession(Guid id, string token, Guid userId, int numberDays, DateTime createdAt, string deviceId, string deviceName) : base(id)
    {
        Token = token;
        UserId = userId;
        ExpiresAt = DateTime.UtcNow.AddDays(numberDays);
        CreatedAt = createdAt;
        IsRevoked = false;
        DeviceId = deviceId;
        DeviceName = deviceName;
    }

    public static Result<UserSession> Create(string token, Guid userId, int numberDays, string deviceId, string deviceName)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return UserSessionErrors.TokenEmpty();
        }

        if (token.Length > MaxTokenLength)
        {
            return UserSessionErrors.TokenTooLong(MaxTokenLength);
        }

        if (string.IsNullOrWhiteSpace(deviceId))
        {
            return UserSessionErrors.DeviceIdEmpty();
        }

        if (deviceId.Length > MaxDeviceIdLength)
        {
            return UserSessionErrors.DeviceIdTooLong(MaxDeviceIdLength);
        }

        if (string.IsNullOrWhiteSpace(deviceName))
        {
            return UserSessionErrors.DeviceNameEmpty();
        }

        if (deviceName.Length > MaxDeviceNameLength)
        {
            return UserSessionErrors.DeviceNameTooLong(MaxDeviceNameLength);
        }

        return new UserSession(Guid.NewGuid(), token, userId, numberDays, DateTime.UtcNow, deviceId, deviceName);
    }

    public void Revoke()
    {
        IsRevoked = true;
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
}
