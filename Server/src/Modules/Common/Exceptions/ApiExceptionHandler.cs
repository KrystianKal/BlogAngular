using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace BlogBackend.Modules.Common.Exceptions;

public class ApiExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        object? errors = null;

        switch (exception)
        {
            case ApiException _:
                httpContext.Response.StatusCode = (int)(exception as ApiException).StatusCode;
                errors = (exception as ApiException).Errors;
                break;
            case ValidationException _:
                var validationException = exception as ValidationException;
                httpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                errors = validationException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
                break;
            default:
                //continue
                return false;
        }

        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new { errors }));

        return true;
    }
}
