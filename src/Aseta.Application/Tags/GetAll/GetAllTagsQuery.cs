using Aseta.Application.Abstractions.Messaging;

namespace Aseta.Application.Tags.GetAll;

public sealed record GetAllTagsQuery : IQuery<TagsResponse>;