using Application.Common.Enums;

namespace Application.Common.Errors;

public sealed record Error(string Code, string Message, ErrorType Type);
public sealed record FieldError(string Field, string Code, string Message);