namespace Application.Customers.Dtos;

public sealed record SaveCustomerRequest
{
    public required string FullName { get; init; }
    public string? CompanyName { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? AlternatePhone { get; init; }
    public required CustomerPreferenceDto Preferences { get; init; }
    public string? Notes { get; init; }
    public bool IsActive { get; init; } = true;
}