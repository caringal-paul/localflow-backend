namespace Application.Users.Dtos;

public sealed record UpdateUserRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required bool IsActive { get; init; }
    public string? Phone { get; init; }
}