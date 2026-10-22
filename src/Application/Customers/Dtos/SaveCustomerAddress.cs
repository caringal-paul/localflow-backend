namespace Application.Customers.Dtos;

public sealed record SaveCustomerAddressRequest
{
    public string? Label { get; init; }
    public required string RecipientName { get; init; }
    public string? RecipientPhone { get; init; }
    public required string Line1 { get; init; }
    public string? Line2 { get; init; }
    public string? Barangay { get; init; }
    public required string City { get; init; }
    public required string Province { get; init; }
    public string? PostalCode { get; init; }
    public string CountryCode { get; init; } = "PH";
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public string? DeliveryInstructions { get; init; }
    public bool IsDefault { get; init; }
}