namespace Application.Customers.Dtos;

public sealed record CustomerAddressDto
{
    public required Guid Id { get; init; }
    public string? Label { get; init; }
    public required string RecipientName { get; init; }
    public string? RecipientPhone { get; init; }
    public required string Line1 { get; init; }
    public string? Line2 { get; init; }
    public string? Barangay { get; init; }
    public required string City { get; init; }
    public required string Province { get; init; }
    public string? PostalCode { get; init; }
    public required string CountryCode { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public string? DeliveryInstructions { get; init; }
    public required bool IsDefault { get; init; }
}