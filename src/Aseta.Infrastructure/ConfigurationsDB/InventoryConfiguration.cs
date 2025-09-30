using Aseta.Domain.Entities.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.ConfigurationsDB;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> inventory)
    {
        inventory.ToTable("Inventories");
        inventory.HasKey(i => i.Id);

        inventory.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(200);

        inventory.Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(1000);

        inventory.Property(i => i.ImageUrl)
            .IsRequired()
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
            .WithMany(i => i.Inventories)
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
    }
}
