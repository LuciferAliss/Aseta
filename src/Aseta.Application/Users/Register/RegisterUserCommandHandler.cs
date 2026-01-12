using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Users.Register;

internal sealed class RegisterUserCommandHandler(
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RegisterUserCommand>
{
    public async Task<Result> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        bool hasUser = await userRepository.ExistsAsync(
            u => u.Email == command.Email,
            cancellationToken: cancellationToken);

        if (hasUser)
        {
            return UserErrors.UserAlreadyExists(command.Email, command.UserName);
        }

        Result<User> createResult = User.Create(
            command.UserName,
            command.Email,
            passwordHasher.Hash(command.Password));

        if (createResult.IsFailure)
        {
            return createResult.Error;
        }

        User user = createResult.Value;

        await userRepository.AddAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
