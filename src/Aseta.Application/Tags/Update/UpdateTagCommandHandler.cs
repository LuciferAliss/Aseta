using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Tags;

namespace Aseta.Application.Tags.Update;

internal sealed class UpdateTagCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateTagCommand>
{
    public async Task<Result> Handle(UpdateTagCommand command, CancellationToken cancellationToken)
    {
        Tag? tag = await tagRepository.GetByIdAsync(command.Id, true, cancellationToken: cancellationToken);

        if (tag is null)
        {
            return TagErrors.NotFound(command.Id);
        }

        Result updateResult = tag.Update(command.Name);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
