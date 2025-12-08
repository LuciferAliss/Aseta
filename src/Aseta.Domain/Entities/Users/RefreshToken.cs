using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Abstractions.Primitives.Results;
using System;

namespace Aseta.Domain.Entities.Users;

public class RefreshToken : Entity
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

    private RefreshToken() { }

    private RefreshToken(Guid id, string token, Guid userId, DateTime expiresAt, DateTime createdAt, string deviceId, string deviceName) : base(id)
    {
        Token = token;
        UserId = userId;
        ExpiresAt = expiresAt;
        CreatedAt = createdAt;
        IsRevoked = false;
        DeviceId = deviceId;
        DeviceName = deviceName;
    }

    public static Result<RefreshToken> Create(string token, Guid userId, DateTime expiresAt, string deviceId, string deviceName)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return RefreshTokenErrors.TokenEmpty();
        }

        if (token.Length > MaxTokenLength)
        {
            return RefreshTokenErrors.TokenTooLong(MaxTokenLength);
        }

        if (string.IsNullOrWhiteSpace(deviceId))
        {
            return RefreshTokenErrors.DeviceIdEmpty();
        }

        if (deviceId.Length > MaxDeviceIdLength)
        {
            return RefreshTokenErrors.DeviceIdTooLong(MaxDeviceIdLength);
        }

        if (string.IsNullOrWhiteSpace(deviceName))
        {
            return RefreshTokenErrors.DeviceNameEmpty();
        }

        if (deviceName.Length > MaxDeviceNameLength)
        {
            return RefreshTokenErrors.DeviceNameTooLong(MaxDeviceNameLength);
        }

        return new RefreshToken(Guid.NewGuid(), token, userId, expiresAt, DateTime.UtcNow, deviceId, deviceName);
    }

    public void Revoke()
    {
        IsRevoked = true;
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
}
