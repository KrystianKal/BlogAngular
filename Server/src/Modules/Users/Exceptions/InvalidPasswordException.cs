using System.Diagnostics.CodeAnalysis;
using System.Net;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Exceptions;

namespace BlogBackend.Modules.Users.Exceptions;

public class InvalidPasswordException() : ApiException(HttpStatusCode.BadRequest,
        new { Password = "At lest 5 characters long; Must contain both upper and lower letters" });
