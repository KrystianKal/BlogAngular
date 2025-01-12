using System.Net;

namespace BlogBackend.Modules.Common.Exceptions;

public class CurrentUserIsNotTheOwnerOfThisResource(object? errors)
    : ApiException(HttpStatusCode.Unauthorized, errors);
