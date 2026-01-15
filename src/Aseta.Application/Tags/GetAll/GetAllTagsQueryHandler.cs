using System;
using System.Windows.Input;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Tags;

namespace Aseta.Application.Tags.GetAll;

public class GetAllTagsQueryHandler(ITagRepository tagRepository) : IQueryHandler<GetAllTagsQuery, TagsResponse>
{
    public async Task<Result<TagsResponse>> Handle(GetAllTagsQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Tag> tags = await tagRepository.GetAllAsync(cancellationToken: cancellationToken);

        var response = new TagsResponse(tags.Select(c => new TagResponse(c.Id, c.Name)).ToList());

        return response;
    }
}
