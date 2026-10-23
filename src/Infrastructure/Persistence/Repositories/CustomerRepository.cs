using Application.Customers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CustomerRepository(ApplicationDbContext db) : ICustomerRepository
{
    public async Task<(IReadOnlyList<Customer> Items, int Total)> SearchAsync(
        string? search, int page, int pageSize, CancellationToken ct)
    {
        var query = db.Customers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = $"%{search.Trim()}%";
            query = query.Where(c =>
                EF.Functions.ILike(c.FullName, term) ||
                EF.Functions.ILike(c.Code, term) ||
                (c.Email != null && EF.Functions.ILike(c.Email, term)) ||
                (c.Phone != null && EF.Functions.ILike(c.Phone, term)));
        }

        var total = await query.CountAsync(ct);
        var items = await query.OrderBy(c => c.FullName)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task<Customer?> GetByCodeAsync(string code, CancellationToken ct) =>
        db.Customers.AsNoTracking()
            .Include(c => c.Addresses)
            .Include(c => c.Preference)
            .FirstOrDefaultAsync(c => c.Code == code, ct);

    public Task<Customer?> GetByCodeForUpdateAsync(string code, CancellationToken ct) =>
        db.Customers
            .Include(c => c.Addresses)
            .Include(c => c.Preference)
            .FirstOrDefaultAsync(c => c.Code == code, ct);

    public async Task AddPreferenceAsync(CustomerPreference preference, CancellationToken ct) =>
        await db.CustomerPreferences.AddAsync(preference, ct);

    public async Task AddAsync(Customer customer, CancellationToken ct) => await db.Customers.AddAsync(customer, ct);

    public void Remove(Customer customer) => db.Customers.Remove(customer);

    public async Task AddAddressAsync(CustomerAddress address, CancellationToken ct) =>
        await db.CustomerAddresses.AddAsync(address, ct);

    public void RemoveAddress(CustomerAddress address) => db.CustomerAddresses.Remove(address);
}