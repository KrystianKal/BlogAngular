using System.Net;

namespace BlogBackend.Modules.Common.Exceptions;

public class ApiException(HttpStatusCode statusCode, object? errors = null) : Exception
{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public object? Errors { get; } = errors;
}
