using Aseta.Domain.Entities.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.Database.ConfigurationsDB;

internal sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> tag)
    {
        tag.ToTable("Tags");
        tag.HasKey(i => i.Id);

        tag.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(Tag.MaxNameLength);

        tag.HasIndex(i => i.Name).IsUnique();
    }
}
