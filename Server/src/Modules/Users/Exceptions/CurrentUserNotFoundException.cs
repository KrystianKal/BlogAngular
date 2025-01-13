using System.Diagnostics.CodeAnalysis;
using System.Net;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Exceptions;

namespace BlogBackend.Modules.Users.Exceptions;

public class CurrentUserNotFoundException(UserId userId)
    :ApiException (HttpStatusCode.InternalServerError, new {User = $"Current user with id: {userId.Value} not found."}),
    IThrowIfNull<User,UserId>
{
    public static void ThrowIfNull([NotNull] User? argument, UserId context)
    {
       if (argument == null) throw new CurrentUserNotFoundException(context); 
    }
}