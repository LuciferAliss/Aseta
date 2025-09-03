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
            user.HasMany(i => i.InventoryUserRoles)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Inventory>(inventory =>
        {
            inventory.ToTable("Inventories");
            inventory.HasKey(i => i.Id);

            inventory.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(200);

            inventory.Property(i => i.Description)
                .IsRequired(false)
                .HasMaxLength(1000);

            inventory.Property(i => i.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(1000);

            inventory.HasMany(i => i.UserRoles)
                .WithOne(i => i.Inventory)
                .HasForeignKey(i => i.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            inventory.HasMany(i => i.Items)
                .WithOne(i => i.Inventory)
                .HasForeignKey(i => i.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            inventory.HasOne(i => i.Category)
                .WithMany(c => c.Inventories)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            inventory.HasMany(i => i.Tags)
                .WithMany(i => i.Inventories);

            inventory.HasOne(i => i.Creator)
                .WithMany(i => i.Inventories)
                .HasForeignKey(i => i.CreatorId)
                .IsRequired();

            inventory.Property(i => i.CustomFields).HasColumnType("jsonb");

            inventory.Property(i => i.CustomIdRules).HasColumnType("jsonb");
        });

        builder.Entity<InventoryUserRole>(iur =>
        {
            iur.ToTable("InventoryUserRoles");
            iur.HasKey(i => new { i.UserId, i.InventoryId, i.Role });

            iur.HasOne(i => i.User)
                .WithMany(i => i.InventoryUserRoles)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            iur.HasOne(i => i.Inventory)
                .WithMany(i => i.UserRoles)
                .HasForeignKey(i => i.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            iur.Property(i => i.Role)
                .HasConversion<string>();
        });

        builder.Entity<Item>(item =>
        {
            item.ToTable("Items");
            item.HasKey(i => i.Id);

            item.Property(i => i.CustomFieldValues).HasColumnType("jsonb");

            item.HasIndex(i => new { i.InventoryId, i.CustomId }).IsUnique();

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
            category.HasKey(i => i.Id);

            category.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100);

            category.HasIndex(i => i.Name).IsUnique();
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
            tag.HasKey(i => i.Id);

            tag.Property(i => i.Id).ValueGeneratedOnAdd();

            tag.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(50);

            tag.HasIndex(i => i.Name).IsUnique();
        });
    }
}