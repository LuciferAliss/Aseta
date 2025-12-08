using Aseta.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.Database.ConfigurationsDB;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> category)
    {
        category.ToTable("Categories");
        category.HasKey(i => i.Id);

        category.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(Category.MaxNameLength);

        category.HasIndex(i => i.Name).IsUnique();
    }
}
