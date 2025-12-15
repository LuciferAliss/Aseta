using System.Security.Cryptography.X509Certificates;
using Aseta.Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.Database.ConfigurationsDB;

internal sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> item)
    {
        item.ToTable("Items");
        item.HasKey(i => i.Id);

        item.Property(i => i.CreatedAt).IsRequired();

        item.Property(i => i.UpdatedAt);

        item.Property(i => i.CustomFieldValues).HasColumnType("jsonb");

        item.HasIndex(i => new { i.InventoryId, i.CustomId }).IsUnique();

        item.HasOne(i => i.Creator)
            .WithMany()
            .HasForeignKey(i => i.CreatorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        item.HasOne(i => i.Updater)
            .WithMany()
            .HasForeignKey(i => i.UpdaterId)
            .OnDelete(DeleteBehavior.Restrict);

        item.HasOne(i => i.Inventory)
            .WithMany(i => i.Items)
            .HasForeignKey(i => i.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
