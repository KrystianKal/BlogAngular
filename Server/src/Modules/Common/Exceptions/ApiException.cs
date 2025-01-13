using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BlogBackend.Modules.Common.Exceptions;

public class ApiException(HttpStatusCode statusCode, object? errors = null) : Exception

{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public object? Errors  { get; } = errors;

    public static void ThrowIfNull([NotNull] object? argument, HttpStatusCode statusCode, object? errors = null)
    {
        if(argument is null) throw new ApiException(statusCode,errors);
    }
}


public interface IThrowIfNull<TArg,TContext>
{
    static abstract void ThrowIfNull([NotNull] TArg? argument, TContext context);
}
