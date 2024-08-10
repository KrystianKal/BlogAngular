using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BlogBackend.Modules.Articles.Domain;

namespace BlogBackend.Modules.Common.Database.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(x => x.TagId);
        builder.Property(x => x.TagId)
            .HasConversion(x => x.Value, x => new TagId(x));
    }
}
