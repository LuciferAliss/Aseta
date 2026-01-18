using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Users.Lock;

internal sealed class LockUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<LockUserCommand>
{
    public async Task<Result> Handle(LockUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(command.UserId, true, cancellationToken);

        if (user is null)
        {
            return UserErrors.NotFound(command.UserId.ToString());
        }

        user.Lock();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
