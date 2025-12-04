using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Tags;
using Aseta.Infrastructure.Database;
using Aseta.Infrastructure.Persistence.Common;

namespace Aseta.Infrastructure.Persistence.Tags;

public sealed class TagRepository(AppDbContext context) : Repository<Tag>(context), ITagRepository;