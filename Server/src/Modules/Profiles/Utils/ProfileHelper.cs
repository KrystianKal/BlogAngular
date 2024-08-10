using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Profiles.Exceptions;
using BlogBackend.Modules.Common.Database;
using BlogBackend.Modules.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogBackend.Modules.Profiles.Utils;

public static class ProfileHelper
{
    public static async Task<Profile> GetUserProfile(string userName, BlogDbContext context, CancellationToken cancellationToken)
    {
        var profileOwner = await context.Users.AsNoTracking()
            .SingleOrDefaultAsync(x => x.Name.ToLower() == userName.ToLower(), cancellationToken);
        if (profileOwner is null)
        {
            throw new ApiException(HttpStatusCode.NotFound, new { User = $"With name: {userName} not found" });
        }

        var profileOwnerId = profileOwner.UserId;

        var profile = await context.Profiles
            .SingleOrDefaultAsync(x => x.UserId == profileOwnerId, cancellationToken);

        if (profile is null)
        {
            throw new ProfileNotFoundException(profileOwnerId.Value.ToString());
        }
        return profile;

    }
}
