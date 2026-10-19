namespace Application.Users.Dtos;

public sealed record CreateUserRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required Guid RoleId { get; init; }
    public string? Phone { get; init; }
}