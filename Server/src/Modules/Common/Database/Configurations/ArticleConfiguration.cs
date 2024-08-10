using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BlogBackend.Modules.Profiles;
using BlogBackend.Modules.Users;
using BlogBackend.Modules.Articles.Domain;

namespace BlogBackend.Modules.Common.Database.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(x => x.ArticleId);
        builder.Property(x => x.ArticleId)
            .HasConversion(x => x.Value, x => new ArticleId(x));
        builder.Property(x => x.AuthorId)
            .HasConversion(x => x.Value, x => new UserId(x));
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .IsRequired();
        builder.Property(x => x.FavoritesCount);

        builder.HasMany(x => x.Comments).WithOne();
        builder.HasMany(x => x.Tags).WithMany(x => x.Articles);
    }
}
