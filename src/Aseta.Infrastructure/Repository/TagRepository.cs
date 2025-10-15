using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Entities.Tags;
using Aseta.Infrastructure.Database;

namespace Aseta.Infrastructure.Repository;

public class TagRepository(AppDbContext context) : Repository<Tag>(context), ITagRepository;