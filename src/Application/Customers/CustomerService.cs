using Application.Common;
using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Customers.Dtos;
using Domain.Entities;
using NetTopologySuite.Geometries;

namespace Application.Customers;

public class CustomerService(
    ICustomerRepository customers,
    IDocumentNumberGenerator numbers,
    IUnitOfWork unitOfWork) : ICustomerService
{
    public async Task<PagedResult<CustomerSummaryDto>> ListAsync(ListCustomersQuery query, CancellationToken ct)
    {
        var (items, total) = await customers.SearchAsync(query.Search, query.Page, query.PageSize, ct);
        return new PagedResult<CustomerSummaryDto>(items.Select(c => c.ToSummary()).ToList(), total, query.Page, query.PageSize);
    }

    public async Task<CustomerDto> GetByCodeAsync(string code, CancellationToken ct) =>
        (await customers.GetByCodeAsync(code, ct) ?? throw new AppException(CustomerErrors.NotFound)).ToDto();

    public async Task<CustomerDto> CreateAsync(SaveCustomerRequest request, CancellationToken ct)
    {
        var customer = new Customer
        {
            Id = Guid.CreateVersion7(),
            Code = await numbers.NextAsync(DocumentSequenceKeys.Customer, ct)
        };
        Apply(customer, request);

        await customers.AddAsync(customer, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return customer.ToDto();
    }

    public async Task<CustomerDto> UpdateAsync(string code, SaveCustomerRequest request, CancellationToken ct)
    {
        var customer = await customers.GetByCodeForUpdateAsync(code, ct) ?? throw new AppException(CustomerErrors.NotFound);
        Apply(customer, request);

        await unitOfWork.SaveChangesAsync(ct);
        return customer.ToDto();
    }

    public async Task DeleteAsync(string code, CancellationToken ct)
    {
        var customer = await customers.GetByCodeForUpdateAsync(code, ct) ?? throw new AppException(CustomerErrors.NotFound);
        customers.Remove(customer);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<CustomerAddressDto> AddAddressAsync(string code, SaveCustomerAddressRequest request, CancellationToken ct)
    {
        var customer = await customers.GetByCodeForUpdateAsync(code, ct) ?? throw new AppException(CustomerErrors.NotFound);

        var address = new CustomerAddress { Id = Guid.CreateVersion7(), CustomerId = customer.Id };
        Apply(address, request);

        // First address is always the default. A new default clears the old one.
        address.IsDefault = request.IsDefault || customer.Addresses.Count == 0;
        if (address.IsDefault)
            foreach (var other in customer.Addresses) other.IsDefault = false;

        await customers.AddAddressAsync(address, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return address.ToDto();
    }

    public async Task<CustomerAddressDto> UpdateAddressAsync(string code, Guid addressId, SaveCustomerAddressRequest request, CancellationToken ct)
    {
        var customer = await customers.GetByCodeForUpdateAsync(code, ct) ?? throw new AppException(CustomerErrors.NotFound);
        var address = customer.Addresses.FirstOrDefault(a => a.Id == addressId) ?? throw new AppException(CustomerErrors.AddressNotFound);

        Apply(address, request);

        if (request.IsDefault && !address.IsDefault)
        {
            foreach (var other in customer.Addresses) other.IsDefault = false;
            address.IsDefault = true;
        }

        await unitOfWork.SaveChangesAsync(ct);
        return address.ToDto();
    }

    public async Task<CustomerPreferenceDto> UpdatePreferencesAsync(string code, SaveCustomerPreferenceRequest request, CancellationToken ct)
    {
        var customer = await customers.GetByCodeForUpdateAsync(code, ct) ?? throw new AppException(CustomerErrors.NotFound);

        var preference = customer.Preference;
        if (preference is null)
        {
            preference = new CustomerPreference { Id = Guid.CreateVersion7(), CustomerId = customer.Id };
            await customers.AddPreferenceAsync(preference, ct);
        }

        preference.AllowEmail = request.AllowEmail;
        preference.AllowSms = request.AllowSms;
        preference.AllowPush = request.AllowPush;

        await unitOfWork.SaveChangesAsync(ct);
        return preference.ToDto();
    }

    public async Task DeleteAddressAsync(string code, Guid addressId, CancellationToken ct)
    {
        var customer = await customers.GetByCodeForUpdateAsync(code, ct) ?? throw new AppException(CustomerErrors.NotFound);
        var address = customer.Addresses.FirstOrDefault(a => a.Id == addressId) ?? throw new AppException(CustomerErrors.AddressNotFound);

        customers.RemoveAddress(address);

        // Promote another address if the default was removed.
        if (address.IsDefault)
        {
            var next = customer.Addresses.FirstOrDefault(a => a.Id != addressId);
            if (next is not null) next.IsDefault = true;
        }

        await unitOfWork.SaveChangesAsync(ct);
    }

    private static void Apply(Customer c, SaveCustomerRequest r)
    {
        c.FullName = r.FullName.Trim();
        c.CompanyName = r.CompanyName?.Trim();
        c.Email = r.Email?.Trim().ToLowerInvariant();
        c.Phone = r.Phone?.Trim();
        c.AlternatePhone = r.AlternatePhone?.Trim();
        c.Notes = r.Notes?.Trim();
        c.IsActive = r.IsActive;
    }

    private static void Apply(CustomerAddress a, SaveCustomerAddressRequest r)
    {
        a.Label = r.Label?.Trim();
        a.RecipientName = r.RecipientName.Trim();
        a.RecipientPhone = r.RecipientPhone?.Trim();
        a.Line1 = r.Line1.Trim();
        a.Line2 = r.Line2?.Trim();
        a.Barangay = r.Barangay?.Trim();
        a.City = r.City.Trim();
        a.Province = r.Province.Trim();
        a.PostalCode = r.PostalCode?.Trim();
        a.CountryCode = r.CountryCode.ToUpperInvariant();
        a.DeliveryInstructions = r.DeliveryInstructions?.Trim();
        a.Location = r is { Latitude: { } lat, Longitude: { } lng }
            ? new Point(lng, lat) { SRID = 4326 }   // Point takes (longitude, latitude)
            : null;
    }
}