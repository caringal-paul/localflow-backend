namespace Application.Auth.Dtos;

public sealed record CurrentUserDto(Guid UserId, string Email, string FullName, Guid? TenantId);