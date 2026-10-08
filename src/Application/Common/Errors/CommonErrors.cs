using Application.Common.Enums;

namespace Application.Common.Errors;

public static class CommonErrors
{
    public static readonly Error ValidationFailed = new("validation.failed", "One or more fields are invalid.", ErrorType.Validation);
    public static readonly Error Malformed = new("request.malformed", "The request is malformed.", ErrorType.Validation);
    public static readonly Error Concurrency = new("concurrency.conflict", "The record was modified by someone else. Reload and try again.", ErrorType.Concurrency);
    public static readonly Error Duplicate = new("conflict.duplicate", "A record with the same value already exists.", ErrorType.Conflict);
    public static readonly Error Unauthorized = new("auth.unauthorized", "Authentication is required.", ErrorType.Unauthorized);
    public static readonly Error Forbidden = new("auth.forbidden", "You do not have permission to perform this action.", ErrorType.Forbidden);
    public static readonly Error NotFound = new("resource.not_found", "The requested resource was not found.", ErrorType.NotFound);
    public static readonly Error MethodNotAllowed = new("request.method_not_allowed", "The HTTP method is not allowed.", ErrorType.MethodNotAllowed);
    public static readonly Error UnsupportedMediaType = new("request.unsupported_media_type", "The content type is not supported.", ErrorType.UnsupportedMediaType);
    public static readonly Error Unexpected = new("server.unexpected", "An unexpected error occurred.", ErrorType.Unexpected);
}