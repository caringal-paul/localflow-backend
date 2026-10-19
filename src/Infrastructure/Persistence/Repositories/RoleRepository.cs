using Application.Roles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RoleRepository(ApplicationDbContext db) : IRoleRepository
{
    public Task<bool> ExistsAsync(Guid id, CancellationToken ct) =>
        db.Roles.AsNoTracking().AnyAsync(r => r.Id == id, ct);
}