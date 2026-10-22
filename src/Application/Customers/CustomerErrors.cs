using Application.Common.Enums;
using Application.Common.Errors;

namespace Application.Customers;

public static class CustomerErrors
{
    public static readonly Error NotFound =
        new("customer.not_found", "Customer not found.", ErrorType.NotFound);

    public static readonly Error AddressNotFound =
        new("customer.address_not_found", "Address not found.", ErrorType.NotFound);
}