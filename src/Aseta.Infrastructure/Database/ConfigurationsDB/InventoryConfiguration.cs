using Aseta.Domain.Entities.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.Database.ConfigurationsDB;

internal sealed class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> inventory)
    {
        inventory.ToTable("inventories");
        inventory.HasKey(i => i.Id);

        inventory.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(Inventory.MaxNameLength);

        inventory.Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(Inventory.MaxDescriptionLength);

        inventory.Property(i => i.ImageUrl)
            .IsRequired();

        inventory.Property(i => i.ItemsCount)
            .IsRequired()
            .HasDefaultValue(0);

        inventory.Property(i => i.IsPublic)
            .IsRequired();

        inventory.Property(i => i.CreatedAt)
            .IsRequired();

        inventory.HasMany(i => i.UserRoles)
            .WithOne(i => i.Inventory)
            .HasForeignKey(i => i.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);

        inventory.HasMany(i => i.Items)
            .WithOne(i => i.Inventory)
            .HasForeignKey(i => i.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);

        inventory.HasOne(i => i.Category)
            .WithMany(i => i.Inventories)
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        inventory.HasMany(i => i.Tags)
            .WithMany(i => i.Inventories);

        inventory.HasOne(i => i.Creator)
            .WithMany(i => i.Inventories)
            .HasForeignKey(i => i.CreatorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        inventory.HasMany(i => i.Comments)
            .WithOne(i => i.Inventory)
            .HasForeignKey(i => i.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);

        inventory.Property(i => i.CustomFields).HasColumnType("jsonb");

        inventory.Property(i => i.CustomIdRules).HasColumnType("jsonb");
    }
}
