using Aseta.Domain.Entities.InventoryRoles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.Database.ConfigurationsDB;

internal sealed class InventoryUserRoleConfiguration : IEntityTypeConfiguration<InventoryRole>
{
    public void Configure(EntityTypeBuilder<InventoryRole> inventoryUserRole)
    {
        inventoryUserRole.ToTable("inventory_user_roles");

        inventoryUserRole.HasKey(i => new { i.UserId, i.InventoryId });

        inventoryUserRole.HasOne(i => i.User)
            .WithMany(i => i.InventoryUserRoles)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        inventoryUserRole.HasOne(iur => iur.Inventory)
            .WithMany(i => i.UserRoles)
            .HasForeignKey(iur => iur.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);

        inventoryUserRole.Property(i => i.Role)
            .IsRequired()
            .HasConversion<string>();
    }
}
