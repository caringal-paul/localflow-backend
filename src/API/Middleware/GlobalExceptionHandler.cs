
using System.Text.Json;
using API.Common;
using Application.Common.Enums;
using Application.Common.Errors;
using Application.Common.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace API.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is OperationCanceledException && context.RequestAborted.IsCancellationRequested)
            return true; // client disconnected, nothing to report

        (Error Error, IReadOnlyList<FieldError>? Fields) result = exception switch
        {
            AppException e => (e.Error, null),
            RequestValidationException e => (CommonErrors.ValidationFailed, e.Errors),
            DbUpdateConcurrencyException => (CommonErrors.Concurrency, null),
            // DbUpdateException { InnerException: PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } pg }
            //     => (UniqueConstraintErrors.Resolve(pg.ConstraintName), null),
            BadHttpRequestException or JsonException => (CommonErrors.Malformed, null),
            _ => (CommonErrors.Unexpected, null)
        };

        if (result.Error.Type == ErrorType.Unexpected)
            logger.LogError(exception, "Unhandled exception");
        else
            logger.LogWarning("Handled {Code}: {Message}", result.Error.Code, result.Error.Message);

        context.Response.StatusCode = result.Error.Type.ToStatusCode();
        await context.Response.WriteAsJsonAsync(ApiResponse.Fail(result.Error, result.Fields), ct);
        return true;
    }
}