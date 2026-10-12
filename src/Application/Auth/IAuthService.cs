using Application.Auth.Dtos;

namespace Application.Auth;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(LoginRequest request, CancellationToken ct);
    Task<CurrentUserDto> GetCurrentAsync(CancellationToken ct);
}