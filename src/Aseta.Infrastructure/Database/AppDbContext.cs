using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Items;
using Aseta.Domain.Entities.Tags;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<UserApplication, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<InventoryUserRole> InventoryUserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserApplication>(user =>
        {
            user.HasMany(u => u.InventoryUserRoles)
                .WithOne(iur => iur.User)
                .HasForeignKey(iur => iur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Inventory>(inventory =>
        {
            inventory.ToTable("Inventories");
            inventory.HasKey(i => i.Id);

            inventory.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(200);

            inventory.HasMany(i => i.UserRoles)
                .WithOne(iur => iur.Inventory)
                .HasForeignKey(iur => iur.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            inventory.HasMany(i => i.Items)
                .WithOne(item => item.Inventory)
                .HasForeignKey(item => item.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            inventory.HasOne(i => i.Category)
                .WithMany(c => c.Inventories)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            inventory.HasMany(i => i.Tags)
                .WithMany(t => t.Inventories);

            inventory.HasOne(i => i.Creator)
                .WithMany(u => u.Inventories)
                .HasForeignKey(i => i.CreatorId)
                .IsRequired();

            inventory.Property(i => i.CustomIdParts).HasColumnType("jsonb");
        });

        builder.Entity<InventoryUserRole>(iur =>
        {
            iur.ToTable("InventoryUserRoles");
            iur.HasKey(i => new { i.UserId, i.InventoryId, i.Role });

            iur.HasOne(i => i.User)
                .WithMany(u => u.InventoryUserRoles)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            iur.HasOne(i => i.Inventory)
                .WithMany(inv => inv.UserRoles)
                .HasForeignKey(i => i.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            iur.Property(i => i.Role)
                .HasConversion<string>();
        });

        builder.Entity<Item>(item =>
        {
            item.ToTable("Items");
            item.HasKey(i => i.Id);

            item.Property(i => i.CustomFields).HasColumnType("jsonb");

            item.HasIndex(item => new { item.InventoryId, item.CustomId }).IsUnique();

            item.HasOne(i => i.Creator)
                .WithMany()
                .HasForeignKey(i => i.CreatorId)
                .IsRequired();

            item.HasOne(i => i.Updater)
                .WithMany()
                .HasForeignKey(i => i.UpdaterId)
                .IsRequired();
        });


        builder.Entity<Category>(category =>
        {
            category.ToTable("Categories");
            category.HasKey(c => c.Id);

            category.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            category.HasIndex(c => c.Name).IsUnique();
        });

        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Clothing" },
            new Category { Id = 3, Name = "Books" },
            new Category { Id = 4, Name = "Games" },
            new Category { Id = 5, Name = "Toys" },
            new Category { Id = 6, Name = "Sports" },
            new Category { Id = 7, Name = "Furniture" },
            new Category { Id = 8, Name = "Other" }
        );

        builder.Entity<Tag>(tag =>
        {
            tag.ToTable("Tags");
            tag.HasKey(t => t.Id);

            tag.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            tag.HasIndex(t => t.Name).IsUnique();
        });
    }
}