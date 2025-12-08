using Aseta.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.Database.ConfigurationsDB;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> user)
    {
        user.ToTable("Users");
        user.HasKey(u => u.Id);

        user.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(User.MaxUserNameLength);

        user.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(User.MaxEmailLength);

        user.HasIndex(u => u.Email).IsUnique();

        user.Property(u => u.PasswordHash).IsRequired();

        user.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>();

        user.Property(u => u.IsLocked)
            .IsRequired();

        user.Property(u => u.CreatedAt).IsRequired();

        user.Property(u => u.UpdatedAt);
    }
}
