using Domain.Entities;

namespace Application.Users;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> IsSessionValidAsync(Guid userId, string? securityStamp, CancellationToken ct);
}