using BlogBackend.Modules.Common.Exceptions;
using System.Net;

namespace BlogBackend.Modules.Articles.Exceptions;

public class CurrentUserIsNotTheAuthorException()
    : ApiException(HttpStatusCode.Unauthorized, new { User = "Is not the author of this resource" });
