using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BlogBackend.Modules.Articles.Domain;

namespace BlogBackend.Modules.Common.Database.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(x => x.CommentId);
        builder.Property(x => x.CommentId)
            .HasConversion(x => x.Value, x => new CommentId(x));
        builder.Property(x => x.AuthorId)
            .HasConversion(x => x.Value, x => new UserId(x));
        builder.Property(x => x.ArticleId)
            .IsRequired();
    }

}
