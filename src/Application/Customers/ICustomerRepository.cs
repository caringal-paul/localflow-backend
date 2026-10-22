using Domain.Entities;

namespace Application.Customers;

public interface ICustomerRepository
{
    Task<(IReadOnlyList<Customer> Items, int Total)> SearchAsync(string? search, int page, int pageSize, CancellationToken ct);
    Task<Customer?> GetByCodeAsync(string code, CancellationToken ct);           // read-only, with addresses
    Task<Customer?> GetByCodeForUpdateAsync(string code, CancellationToken ct);  // tracked, with addresses
    Task AddAsync(Customer customer, CancellationToken ct);
    void Remove(Customer customer);
    Task AddAddressAsync(CustomerAddress address, CancellationToken ct);
    void RemoveAddress(CustomerAddress address);
}