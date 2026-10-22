using Application.Customers.Dtos;
using Domain.Entities;

namespace Application.Customers;

public static class CustomerMapper
{
    public static CustomerSummaryDto ToSummary(this Customer c) =>
        new(c.Id, c.Code, c.FullName, c.Phone, c.IsActive);

    public static CustomerDto ToDto(this Customer c) => new()
    {
        Id = c.Id, Code = c.Code, FullName = c.FullName, CompanyName = c.CompanyName,
        Email = c.Email, Phone = c.Phone, AlternatePhone = c.AlternatePhone, Notes = c.Notes,
        IsActive = c.IsActive,
        Addresses = c.Addresses.OrderByDescending(a => a.IsDefault).Select(a => a.ToDto()).ToList()
    };

    public static CustomerAddressDto ToDto(this CustomerAddress a) => new()
    {
        Id = a.Id, Label = a.Label, RecipientName = a.RecipientName, RecipientPhone = a.RecipientPhone,
        Line1 = a.Line1, Line2 = a.Line2, Barangay = a.Barangay, City = a.City, Province = a.Province,
        PostalCode = a.PostalCode, CountryCode = a.CountryCode,
        Latitude = a.Location?.Y, Longitude = a.Location?.X,
        DeliveryInstructions = a.DeliveryInstructions, IsDefault = a.IsDefault
    };
}