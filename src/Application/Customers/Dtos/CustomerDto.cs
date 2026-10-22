namespace Application.Customers.Dtos;

public sealed record CustomerSummaryDto(Guid Id, string Code, string FullName, string? Phone, bool IsActive);

public sealed record CustomerDto
{
    public required Guid Id { get; init; }
    public required string Code { get; init; }
    public required string FullName { get; init; }
    public string? CompanyName { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? AlternatePhone { get; init; }
    public string? Notes { get; init; }
    public required bool IsActive { get; init; }
    public required IReadOnlyList<CustomerAddressDto> Addresses { get; init; }
}