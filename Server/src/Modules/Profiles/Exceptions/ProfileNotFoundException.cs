using System.Diagnostics.CodeAnalysis;
using BlogBackend.Modules.Common.Exceptions;
using System.Net;
using BlogBackend.Modules.Common;

namespace BlogBackend.Modules.Profiles.Exceptions;

// ReSharper disable once ClassNeverInstantiated.Global
public class ProfileNotFoundException(UserId userId)
    : ApiException(HttpStatusCode.NotFound, new { Profile = $"Profile with UserId: \"{userId}\" not found." }),
        IThrowIfNull<Profile,UserId>
{
    public static void ThrowIfNull([NotNull] Profile? argument, UserId context)
    {
        if(argument == null) throw new ProfileNotFoundException(context);
    }
}
    
