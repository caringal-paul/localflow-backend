namespace Application.Common.Errors;

public class AppException(Error error) : Exception(error.Message)
{
    public Error Error { get; } = error;
}

public class RequestValidationException(IReadOnlyList<FieldError> errors)
    : Exception(CommonErrors.ValidationFailed.Message)
{
    public IReadOnlyList<FieldError> Errors { get; } = errors;
}