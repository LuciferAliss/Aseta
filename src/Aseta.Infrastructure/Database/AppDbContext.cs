using Aseta.Domain.Abstractions.Primitives.Entities;
using Aseta.Domain.Abstractions.Primitives.Events;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Categories;
using Aseta.Domain.Entities.Comments;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.InventoryRoles;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Tags;
using Aseta.Domain.Entities.Users;
using Aseta.Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Aseta.Infrastructure.Database;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IDomainEventsDispatcher domainEventsDispatcher,
    ILogger<AppDbContext> logger) : DbContext(options)
{
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<InventoryRole> InventoryRoles { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);
        await PublishDomainEventsAsync(cancellationToken);
        return result;
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEvents = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents.ToList();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        Result dispatchResult = await domainEventsDispatcher.DispatchAsync(domainEvents, cancellationToken);

        if (dispatchResult.IsFailure)
        {
            logger.LogWarning("Post-transaction domain event handlers failed. {Error}", dispatchResult.Error);
        }
    }
}