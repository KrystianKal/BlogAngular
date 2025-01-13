using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Profiles.Exceptions;
using BlogBackend.Modules.Common.Database;
using Microsoft.EntityFrameworkCore;
using System.Net;
using BlogBackend.Modules.Users.Exceptions;

namespace BlogBackend.Modules.Profiles.Utils;

public static class ProfileHelper
{
    public static async Task<Profile> GetUserProfile(string userName, BlogDbContext context, CancellationToken cancellationToken)
    {
        var profileOwner = await context.Users.AsNoTracking()
            .SingleOrDefaultAsync(x => x.Name.Equals(userName, StringComparison.CurrentCultureIgnoreCase), cancellationToken);
        
        UserNotFoundException.ThrowIfNull(profileOwner,userName);
        
        var profileOwnerId = profileOwner.UserId;
        var profile = await context.Profiles
            .SingleOrDefaultAsync(x => x.UserId == profileOwnerId, cancellationToken);

        ProfileNotFoundException.ThrowIfNull(profile,profileOwnerId);
        return profile;
    }
}
