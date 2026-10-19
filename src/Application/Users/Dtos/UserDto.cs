namespace Application.Users.Dtos;

public sealed record UserDto
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? Phone { get; init; }
    public required bool IsActive { get; init; }
    public required IReadOnlyList<Guid> RoleIds { get; init; }
}