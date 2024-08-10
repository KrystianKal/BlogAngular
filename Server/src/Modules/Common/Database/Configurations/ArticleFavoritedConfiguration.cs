using BlogBackend.Modules.Articles.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogBackend.Modules.Common.Database.Configurations;

public class ArticleFavoritedConfiguration : IEntityTypeConfiguration<ArticleFavorited>
{
    public void Configure(EntityTypeBuilder<ArticleFavorited> builder)
    {
        builder.HasKey(x => new { x.UserId, x.ArticleId });

        builder
            .HasOne(x => x.Article)
            .WithMany(x => x.ArticleFavoriteds)
            .HasForeignKey(x => x.ArticleId);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.Property(x => x.ArticleId)
            .HasColumnName("ArticleId");
    }
}
