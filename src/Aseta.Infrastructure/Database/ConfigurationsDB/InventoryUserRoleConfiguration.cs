using Aseta.Domain.Entities.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.Database.ConfigurationsDB;

public class InventoryUserRoleConfiguration : IEntityTypeConfiguration<Domain.Entities.Inventories.InventoryRole>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Inventories.InventoryRole> builder)
    {
        builder.ToTable("InventoryUserRoles");
        builder.HasKey(i => new { i.UserId, i.InventoryId, i.Role });

        builder.HasOne(i => i.User)
            .WithMany(i => i.InventoryUserRoles)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.Property(i => i.Role)
            .HasConversion<string>();
    }
}
