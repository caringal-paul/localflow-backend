namespace Application.Auth.Dtos;

public sealed record LoginRequest(string Email, string Password);
public sealed record LoginResult(Guid UserId, Guid TenantId, string SecurityStamp, string Email, string FullName);