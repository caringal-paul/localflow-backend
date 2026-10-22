using Application.Common;
using Application.Customers.Dtos;

namespace Application.Customers;

public interface ICustomerService
{
    Task<PagedResult<CustomerSummaryDto>> ListAsync(ListCustomersQuery query, CancellationToken ct);
    Task<CustomerDto> GetByCodeAsync(string code, CancellationToken ct);
    Task<CustomerDto> CreateAsync(SaveCustomerRequest request, CancellationToken ct);
    Task<CustomerDto> UpdateAsync(string code, SaveCustomerRequest request, CancellationToken ct);
    Task DeleteAsync(string code, CancellationToken ct);
    Task<CustomerAddressDto> AddAddressAsync(string code, SaveCustomerAddressRequest request, CancellationToken ct);
    Task<CustomerAddressDto> UpdateAddressAsync(string code, Guid addressId, SaveCustomerAddressRequest request, CancellationToken ct);
    Task DeleteAddressAsync(string code, Guid addressId, CancellationToken ct);
}