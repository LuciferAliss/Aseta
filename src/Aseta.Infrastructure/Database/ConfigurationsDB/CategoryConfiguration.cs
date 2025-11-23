using Aseta.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.Database.ConfigurationsDB;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.CategoryName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(i => i.CategoryName).IsUnique();
    }
}
