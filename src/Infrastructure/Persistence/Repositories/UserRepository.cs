using Application.Users;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext db) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
        => db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        => db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<bool> IsSessionValidAsync(Guid userId, string? securityStamp, CancellationToken ct)
        => db.Users.AsNoTracking()
            .AnyAsync(u => u.Id == userId && u.IsActive && u.SecurityStamp == securityStamp, ct);
}