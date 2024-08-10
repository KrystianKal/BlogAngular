using System.Net;

namespace BlogBackend.Modules.Common.Exceptions;

public class CurrentUserIsNotTheOwnerOfThisResource(object? Errors)
    : ApiException(HttpStatusCode.Unauthorized, Errors);
