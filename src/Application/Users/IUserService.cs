using Application.Users.Dtos;

namespace Application.Users;

public interface IUserService
{
    Task<IReadOnlyList<UserDto>> ListAsync(CancellationToken ct);
    Task<UserDto> GetByIdAsync(Guid id, CancellationToken ct);
    Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken ct);
    Task<UserDto> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}