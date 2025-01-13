using System.Diagnostics.CodeAnalysis;
using System.Net;
using BlogBackend.Modules.Common.Exceptions;

namespace BlogBackend.Modules.Users.Exceptions;

public class UserNotFoundException(string email)
    : ApiException(HttpStatusCode.NotFound, new { User = $"User with email: {email} not found." })
        , IThrowIfNull<User, string>
{
    public static void ThrowIfNull([NotNull]User? argument, string context)
    {
        if (argument is null) throw new  UserNotFoundException(context);
    }
}
