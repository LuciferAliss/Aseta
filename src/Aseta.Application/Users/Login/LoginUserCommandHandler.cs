using System;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;
using Microsoft.Extensions.Options;

namespace Aseta.Application.Users.Login;

internal sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    ITokenProvider tokenProvider) : ICommandHandler<LoginUserCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.FirstOrDefaultAsync(
            u => u.Email == command.Email,
            cancellationToken: cancellationToken);

        if (user is null)
        {
            return UserErrors.NotFoundByEmail(command.Email);
        }

        bool passwordMatch = passwordHasher.Verify(command.Password, user.PasswordHash);

        if (!passwordMatch)
        {
            return UserErrors.PasswordIncorrect();
        }

        string accessToken = tokenProvider.CreateAccessToken(user);

        RefreshToken? refreshToken = await refreshTokenRepository.FirstOrDefaultAsync(
            rt => rt.UserId == user.Id &&
            rt.DeviceId == command.DeviceId &&
            !rt.IsRevoked &&
            DateTime.UtcNow < rt.ExpiresAt,
            cancellationToken: cancellationToken);

        if (refreshToken is not null)
        {
            return new LoginResponse(accessToken, refreshToken.Token);
        }

        Result<RefreshToken> refreshTokenResult = tokenProvider.CreateRefreshToken(user, command.DeviceId, command.DeviceName);

        if (refreshTokenResult.IsFailure)
        {
            return refreshTokenResult.Error;
        }

        refreshToken = refreshTokenResult.Value;

        await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponse(accessToken, refreshToken.Token);
    }
}
