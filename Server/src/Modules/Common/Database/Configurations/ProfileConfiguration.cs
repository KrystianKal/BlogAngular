using BlogBackend.Modules.Common;
using BlogBackend.Modules.Users;
using BlogBackend.Modules.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogBackend.Modules.Common.Database.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(x => x.ProfileId);
        builder.Property(x => x.ProfileId)
            .HasConversion(x => x.Value, x => new ProfileId(x));
        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => new UserId(x))
            .IsRequired();
        builder.Property(x => x.Image)
            .HasConversion(x => x.Value, x => new ProfileImage(x));
    }
}
