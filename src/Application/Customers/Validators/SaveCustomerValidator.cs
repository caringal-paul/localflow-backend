using Application.Customers.Dtos;
using FluentValidation;

namespace Application.Customers.Validators;

public class SaveCustomerRequestValidator : AbstractValidator<SaveCustomerRequest>
{
    public SaveCustomerRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CompanyName).MaximumLength(200);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(256).When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.Phone).MaximumLength(32);
        RuleFor(x => x.AlternatePhone).MaximumLength(32);
        RuleFor(x => x.Notes).MaximumLength(2000);
    }
}
