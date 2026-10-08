using Application.Common.Errors;
using Application.Common.Responses;
using Microsoft.AspNetCore.Diagnostics;

namespace API.Middleware;

public static class StatusCodeResponseWriter
{
    public static Task WriteAsync(StatusCodeContext context)
    {
        var response = context.HttpContext.Response;

        Error? error = response.StatusCode switch
        {
            StatusCodes.Status401Unauthorized => CommonErrors.Unauthorized,
            StatusCodes.Status403Forbidden => CommonErrors.Forbidden,
            StatusCodes.Status404NotFound => CommonErrors.NotFound,
            StatusCodes.Status405MethodNotAllowed => CommonErrors.MethodNotAllowed,
            StatusCodes.Status415UnsupportedMediaType => CommonErrors.UnsupportedMediaType,
            _ => null
        };

        return error is null
            ? Task.CompletedTask
            : response.WriteAsJsonAsync(ApiResponse.Fail(error));
    }
}