using System.Diagnostics;
using Application.Common.Enums;
using Application.Common.Errors;

namespace Application.Common.Responses;

public sealed record ErrorInfo(string Code, ErrorType Type);

public record ApiResponse
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public ErrorInfo? Error { get; init; }
    public IReadOnlyList<FieldError>? Errors { get; init; }
    public string? TraceId { get; init; }

    public static ApiResponse<T> Ok<T>(T data, string? message = null)
        => new() { Success = true, Data = data, Message = message, TraceId = CurrentTraceId() };

    public static ApiResponse Ok()
        => new() { Success = true, TraceId = CurrentTraceId() };

    public static ApiResponse Fail(Error error, IReadOnlyList<FieldError>? fieldErrors = null)
        => new()
        {
            Success = false,
            Message = error.Message,
            Error = new ErrorInfo(error.Code, error.Type),
            Errors = fieldErrors,
            TraceId = CurrentTraceId()
        };

    private static string? CurrentTraceId() => Activity.Current?.TraceId.ToString();
}

public record ApiResponse<T> : ApiResponse
{
    public T? Data { get; init; }
}