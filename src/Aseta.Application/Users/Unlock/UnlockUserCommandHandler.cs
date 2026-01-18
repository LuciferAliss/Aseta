using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Users.Unlock;

internal sealed class UnlockUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UnlockUserCommand>
{
    public async Task<Result> Handle(UnlockUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(command.UserId, true, cancellationToken);

        if (user is null)
        {
            return UserErrors.NotFound(command.UserId.ToString());
        }

        user.Unlock();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
