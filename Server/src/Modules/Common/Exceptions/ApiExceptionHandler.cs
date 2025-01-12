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
            case ApiException apiException:
                httpContext.Response.StatusCode = (int)apiException.StatusCode;
                errors = apiException.Errors;
                break;
            case ValidationException validationException:
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
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new { errors }), cancellationToken);

        return true;
    }
}
