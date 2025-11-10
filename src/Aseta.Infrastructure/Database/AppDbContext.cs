using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Tags;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Database;

internal sealed class AppDbContext(
    DbContextOptions<AppDbContext> options) : IdentityDbContext<UserApplication>(options)
{
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<InventoryRole> InventoryUserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}