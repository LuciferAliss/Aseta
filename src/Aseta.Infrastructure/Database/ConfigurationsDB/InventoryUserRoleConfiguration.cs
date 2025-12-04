using Aseta.Domain.Entities.UserRoles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.Database.ConfigurationsDB;

internal sealed class InventoryUserRoleConfiguration : IEntityTypeConfiguration<InventoryRole>
{
    public void Configure(EntityTypeBuilder<InventoryRole> builder)
    {
        builder.ToTable("InventoryUserRoles");

        builder.HasKey(i => new { i.UserId, i.InventoryId });

        builder.HasOne(i => i.User)
            .WithMany(i => i.InventoryUserRoles)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Inventory)
            .WithMany(i => i.UserRoles)
            .HasForeignKey(i => i.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(i => i.Role)
            .IsRequired()
            .HasConversion<string>();
    }
}
