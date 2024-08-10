using BlogBackend.Modules.Common.Exceptions;
using System.Net;

namespace BlogBackend.Modules.Profiles.Exceptions;

public class ProfileNotFoundException(string UserId)
    : ApiException(HttpStatusCode.NotFound, new { Profile = $"Profile of user with id: \"{UserId}\" not found." })
{
}
