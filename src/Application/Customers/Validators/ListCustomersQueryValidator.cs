using Application.Customers.Dtos;
using FluentValidation;

namespace Application.Customers.Validators;

public class ListCustomersQueryValidator : AbstractValidator<ListCustomersQuery>
{
    public ListCustomersQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}