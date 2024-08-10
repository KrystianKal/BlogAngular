using BlogBackend.Modules.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogBackend.Modules.Common.Database.Configurations;

public class ProfileFollowConfiguration : IEntityTypeConfiguration<ProfileFollow>
{
    public void Configure(EntityTypeBuilder<ProfileFollow> builder)
    {
        // Define the composite key using the foreign key properties
        builder.HasKey(x => new { x.FollowerId, x.FollowingId });

        // Configure the Follower relationship
        builder.HasOne(x => x.Follower)
            .WithMany(x => x.Following)
            .HasForeignKey(x => x.FollowerId);

        // Configure the Following relationship
        builder.HasOne(x => x.Following)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.FollowingId);
    }
}
