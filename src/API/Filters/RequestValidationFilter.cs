using System.Text.RegularExpressions;
using Application.Common.Errors;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

public partial class RequestValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
            throw new AppException(CommonErrors.Malformed);

        var failures = new List<ValidationFailure>();
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator) continue;

            var result = await validator.ValidateAsync(
                new ValidationContext<object>(argument), context.HttpContext.RequestAborted);
            failures.AddRange(result.Errors);
        }

        if (failures.Count > 0)
            throw new RequestValidationException(failures.Select(ToFieldError).ToList());

        await next();
    }

    private static FieldError ToFieldError(ValidationFailure f)
        => new(ToCamelPath(f.PropertyName), ToCode(f.ErrorCode), f.ErrorMessage);

    // FluentValidation's default codes are validator names. Narrow them to stable codes.
    // Custom rules use .WithErrorCode("snake_case_code") and pass through.
    private static string ToCode(string errorCode) => errorCode switch
    {
        "NotEmptyValidator" or "NotNullValidator" => "required",
        "MaximumLengthValidator" => "too_long",
        "MinimumLengthValidator" => "too_short",
        "EmailValidator" or "AspNetCoreCompatibleEmailValidator" => "invalid_email",
        "RegularExpressionValidator" => "invalid_format",
        "GreaterThanValidator" or "GreaterThanOrEqualValidator" or "LessThanValidator"
            or "LessThanOrEqualValidator" or "InclusiveBetweenValidator" or "ExclusiveBetweenValidator" => "out_of_range",
        _ when CustomCode().IsMatch(errorCode) => errorCode,
        _ => "invalid"
    };

    private static string ToCamelPath(string path)
        => string.Join('.', path.Split('.').Select(s => s.Length == 0 ? s : char.ToLowerInvariant(s[0]) + s[1..]));

    [GeneratedRegex("^[a-z]+(_[a-z]+)*$")]
    private static partial Regex CustomCode();
}