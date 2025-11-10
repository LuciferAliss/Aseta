using Aseta.Domain.Entities.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.Database.ConfigurationsDB;

internal sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id).ValueGeneratedOnAdd();

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(i => i.Name).IsUnique();
    }
}
