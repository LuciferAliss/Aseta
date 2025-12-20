using System;
using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Abstractions.Services;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;
using Microsoft.Extensions.Options;

namespace Aseta.Application.Users.Login;

internal sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUserSessionRepository userSessionRepository,
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

        string accessToken = "";

        UserSession? userSession = await userSessionRepository.FirstOrDefaultAsync(
            rt => rt.UserId == user.Id &&
            rt.DeviceId == command.DeviceId &&
            !rt.IsRevoked &&
            DateTime.UtcNow < rt.ExpiresAt,
            cancellationToken: cancellationToken);

        if (userSession is not null)
        {
            accessToken = tokenProvider.CreateAccessToken(user, userSession.Id);
            return new LoginResponse(accessToken, userSession);
        }

        (string refreshToken, int expiresInRefreshToken) = tokenProvider.CreateRefreshToken();

        Result<UserSession> userSessionResult = UserSession.Create(
            refreshToken,
            user.Id,
            expiresInRefreshToken,
            command.DeviceId, command.DeviceName);

        if (userSessionResult.IsFailure)
        {
            return userSessionResult.Error;
        }

        userSession = userSessionResult.Value;

        accessToken = tokenProvider.CreateAccessToken(user, userSession.Id);

        await userSessionRepository.AddAsync(userSession, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponse(accessToken, userSession);
    }
}