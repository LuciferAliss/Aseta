using Aseta.Domain.Entities.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aseta.Infrastructure.Database.ConfigurationsDB;

internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> comment)
    {
        comment.ToTable("comments");
        comment.HasKey(c => c.Id);

        comment.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(Comment.MaxContentLength);

        comment.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        comment.HasOne(c => c.Inventory)
            .WithMany(i => i.Comments)
            .HasForeignKey(c => c.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);

        comment.HasMany(c => c.Likes)
            .WithOne(l => l.Comment)
            .HasForeignKey(l => l.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
