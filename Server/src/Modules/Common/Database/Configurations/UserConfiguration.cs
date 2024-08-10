using BlogBackend.Modules.Common;
using BlogBackend.Modules.Profiles;
using BlogBackend.Modules.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogBackend.Modules.Common.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => new UserId(x));
        builder.HasOne<Profile>()
            .WithOne(x=>x.User)
            .HasForeignKey<Profile>(p => p.UserId)
            .IsRequired();
        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new Email(x));
    }
}
