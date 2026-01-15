using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Tags;

namespace Aseta.Application.Tags.Create;

internal sealed class CreateTagCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateTagCommand>
{
    public async Task<Result> Handle(CreateTagCommand command, CancellationToken cancellationToken)
    {
        Tag tag = await tagRepository.GetByNameAsync(command.Name, cancellationToken: cancellationToken);

        if (tag is not null)
        {
            return TagErrors.AlreadyExists(command.Name);
        }

        Result<Tag> tagResult = Tag.Create(command.Name);

        if (tagResult.IsFailure)
        {
            return tagResult.Error;
        }

        tag = tagResult.Value;

        await tagRepository.AddAsync(tag, cancellationToken: cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
