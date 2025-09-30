using Aseta.Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.ConfigurationsDB;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.CustomFieldValues).HasColumnType("jsonb");

        builder.HasIndex(i => new { i.InventoryId, i.CustomId }).IsUnique();

        builder.HasOne(i => i.Creator)
            .WithMany()
            .HasForeignKey(i => i.CreatorId)
            .IsRequired();

        builder.HasOne(i => i.Updater)
            .WithMany()
            .HasForeignKey(i => i.UpdaterId)
            .IsRequired();
    }
}
